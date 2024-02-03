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

    int spawnX = 0;
    int spawnY = 0;

    TiledMap _tiledMap;
    TiledMapRenderer _tiledMapRenderer;

    private AnimatedSprite _motwSprite;
    private Vector2 _motwPosition;

    bool hasJumped;

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

        var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, _graphics.PreferredBackBufferWidth / 3, _graphics.PreferredBackBufferHeight / 3);
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

        foreach (var item in _tiledMap.Properties)
        {
            if (item.Key == "spawnX")
                spawnX = int.Parse(item.Value) * 16;
            if (item.Key == "spawnY")
                spawnY = int.Parse(item.Value) * 16;
        }

        Debug.WriteLine(spawnX);
        Debug.WriteLine(spawnY);

        _motwPosition = new Vector2(spawnX, spawnY);
        _motwSprite = sprite;
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        kState = Keyboard.GetState();
        mState = Mouse.GetState();

        var deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

        var walkSpeed = deltaSeconds * 300;
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

        if (kState.IsKeyDown(Keys.T))
            _motwPosition = new Vector2(spawnX, spawnY);

        if (kState.IsKeyDown(Keys.Space) && !hasJumped)
        {
            animation = "jump";
            hasJumped = true;
        }


        _motwPosition.X += changeX;
        _motwPosition.Y += changeY;

        if (_motwPosition.X <= 1 || _motwPosition.Y <= 1)
        {
            _motwPosition.X -= changeX;
            _motwPosition.Y -= changeY;
        }

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
                TileY = (_motwPosition.Y + 20) / tileWidth;
                tile = _tiledMap.TileLayers[1].GetTile((ushort)TileX, (ushort)TileY);

                if (tile.X == 0 && tile.Y == 0)
                {
                    TileX = (_motwPosition.X + 32) / tileWidth;
                    tile = _tiledMap.TileLayers[1].GetTile((ushort)TileX, (ushort)TileY);

                    if (tile.X == 0 && tile.Y == 0 && !hasJumped)
                        _motwPosition.Y += 5;
                }
            }
        }

        else if (tile.X != 0 && tile.Y != 0)
        {
            _motwPosition.X -= changeX;
            _motwPosition.Y -= changeY;
            hasJumped = false;
        }

        _motwSprite.Play(animation);

        _camera.LookAt(_motwPosition);

        _motwSprite.Update(deltaSeconds);
        _tiledMapRenderer.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        var transformMatrix = _camera.GetViewMatrix();
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: transformMatrix);

        _spriteBatch.Draw(_motwSprite, _motwPosition, 0, new Vector2(2, 2));

        _spriteBatch.DrawLine(_motwPosition, new Vector2(_motwPosition.X + 32, _motwPosition.Y + 20), Color.White);

        _tiledMapRenderer.Draw(_tiledMap.GetLayer("terrain"), transformMatrix);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
