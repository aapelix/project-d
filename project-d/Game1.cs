using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace project_d;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    MouseState mState;
    KeyboardState kState;

    List<ScaledSprite> sprites;

    Player player;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        sprites = new List<ScaledSprite>();

        Texture2D texture = Content.Load<Texture2D>("character");
        Texture2D ground = Content.Load<Texture2D>("ground");

        sprites.Add(new ScaledSprite(ground, new Vector2(350, 400)));
        sprites.Add(new ScaledSprite(ground, new Vector2(400, 400)));
        sprites.Add(new ScaledSprite(ground, new Vector2(450, 400)));

        player = new Player(texture, new Vector2(400, 0));
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        kState = Keyboard.GetState();
        mState = Mouse.GetState();

        foreach (ScaledSprite sprite in sprites)
        {
            sprite.Update();

            if (sprite != player && sprite.Rect.Intersects(player.Rect))
            {

            }
        }

        player.Update(gameTime, sprites);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        foreach (ScaledSprite sprite in sprites)
            _spriteBatch.Draw(sprite.texture, sprite.pos, Color.White);

        _spriteBatch.Draw(player.texture, player.Rect, Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
