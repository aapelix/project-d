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

namespace project_d;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    MouseState mState;
    KeyboardState kState;

    TiledMap _tiledMap;
    TiledMapRenderer _tiledMapRenderer;

    private AnimatedSprite _motwSprite;
    private Vector2 _motwPosition;

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

        _tiledMap = Content.Load<TiledMap>("tiledmap");
        _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);

        var spriteSheet = Content.Load<SpriteSheet>("spritesheet.sf", new JsonContentLoader());
        var sprite = new AnimatedSprite(spriteSheet);

        sprite.Play("idle");
        _motwPosition = new Vector2(100, 10);
        _motwSprite = sprite;
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        kState = Keyboard.GetState();
        mState = Mouse.GetState();

        var deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

        var walkSpeed = deltaSeconds * 256;
        var gravity = deltaSeconds * 200;

        var keyboardState = Keyboard.GetState();
        var animation = "idle";
        float changeX = 0;
        float changeY = 0;

        if (kState.IsKeyDown(Keys.A))
        {
            animation = "run2";
            changeX -= walkSpeed;
        }

        if (kState.IsKeyDown(Keys.D))
        {
            animation = "run";
            changeX += walkSpeed;
        }

        if (kState.IsKeyDown(Keys.S))
        {
            changeY += walkSpeed;
        }

        if (kState.IsKeyDown(Keys.W))
        {
            changeY -= walkSpeed;
        }

        _motwPosition.X += changeX;
        _motwPosition.Y += changeY;

        // Collision

        var tileWidth = 16;
        var TileX = _motwPosition.X / tileWidth;
        var TileY = _motwPosition.Y / tileWidth;

        var tile = _tiledMap.TileLayers[1].GetTile((ushort)TileX, (ushort)TileY);

        if (tile.X == 0 && tile.Y == 0)
        {
            TileX = (_motwPosition.X + 32) / tileWidth;
            tile = _tiledMap.TileLayers[1].GetTile((ushort)TileX, (ushort)TileY);

            if (tile.X == 0 && tile.Y == 0)
            {
                TileX = _motwPosition.X / tileWidth;
                TileY = (_motwPosition.Y + 40) / tileWidth;
                tile = _tiledMap.TileLayers[1].GetTile((ushort)TileX, (ushort)TileY);

                if (tile.X == 0 && tile.Y == 0)
                {
                    TileX = (_motwPosition.X + 32) / tileWidth;
                    tile = _tiledMap.TileLayers[1].GetTile((ushort)TileX, (ushort)TileY);
                }
            }
        }

        if (tile.X != 0 && tile.Y != 0)
        {
            _motwPosition.X -= changeX;
            _motwPosition.Y -= changeY;
        }



        _motwSprite.Play(animation);

        if (keyboardState.IsKeyDown(Keys.R))
            _camera.ZoomIn(deltaSeconds);

        if (keyboardState.IsKeyDown(Keys.F))
            _camera.ZoomOut(deltaSeconds);

        _motwSprite.Update(deltaSeconds);
        _tiledMapRenderer.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        var transformMatrix = _camera.GetViewMatrix();
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: transformMatrix);

        _spriteBatch.Draw(_motwSprite, _motwPosition, 0, new Vector2(5, 5));
        _tiledMapRenderer.Draw();

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
