using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Content;
using MonoGame.Extended.Collisions;
using System;

namespace project_d;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    MouseState mState;
    KeyboardState kState;

    readonly int tileWidth;

    TiledMap _tiledMap;
    TiledMapRenderer _tiledMapRenderer;

    private AnimatedSprite _motwSprite;
    private Vector2 _motwPosition;

    private CollisionComponent _collisionHandler;

    List<ScaledSprite> sprites;

    SpriteEffects s = SpriteEffects.FlipHorizontally;

    private OrthographicCamera _camera;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        if (GraphicsDevice == null)
        {
            _graphics.ApplyChanges();
        }

        _graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
        _graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;
        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();

        var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        _camera = new OrthographicCamera(viewportAdapter);
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        sprites = new List<ScaledSprite>();

        _tiledMap = Content.Load<TiledMap>("tiledmap");
        _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);

        var spriteSheet = Content.Load<SpriteSheet>("hoodieman.sf", new JsonContentLoader());
        var sprite = new AnimatedSprite(spriteSheet);

        sprite.Play("idle");
        _motwPosition = new Vector2(0, 0);
        _motwSprite = sprite;
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        kState = Keyboard.GetState();
        mState = Mouse.GetState();

        var deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var walkSpeed = deltaSeconds * 128;
        var keyboardState = Keyboard.GetState();
        var animation = "idle";
        float changeX = 0;

        if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
        {
            animation = "run";
            changeX -= walkSpeed;
        }

        if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
        {
            animation = "run";
            changeX += walkSpeed;
        }

        if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
        {
            animation = "run";
            _motwPosition.Y += 1;
        }

        if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
        {
            animation = "run";
            _motwPosition.Y -= 1;
        }

        _motwPosition.X += changeX;

        var TileX = _motwPosition.X / 16;
        var TileY = _motwPosition.Y / 16;

        var tile = _tiledMap.TileLayers[2].GetTile((ushort)TileX, (ushort)TileY);

        if (tile.X != 0 && tile.Y != 0)
            Debug.WriteLine(tile.X + " " + tile.Y);






        _motwSprite.Play(animation);

        if (keyboardState.IsKeyDown(Keys.R))
            _camera.ZoomIn(deltaSeconds);

        if (keyboardState.IsKeyDown(Keys.F))
            _camera.ZoomOut(deltaSeconds);

        _motwSprite.Update(deltaSeconds);

        foreach (ScaledSprite sprite in sprites)
        {
            sprite.Update();
        }

        _tiledMapRenderer.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        var transformMatrix = _camera.GetViewMatrix();
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: transformMatrix);

        foreach (ScaledSprite sprite in sprites)
            _spriteBatch.Draw(sprite.texture, sprite.pos, Color.White);

        _spriteBatch.Draw(_motwSprite, _motwPosition);

        _tiledMapRenderer.Draw();

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
