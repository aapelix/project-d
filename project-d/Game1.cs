using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace project_d;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    Texture2D spriteSheet;


    MouseState mState;
    KeyboardState kState;

    List<ScaledSprite> sprites;

    AnimationManager am;

    int posX;
    int posY;

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

        spriteSheet = Content.Load<Texture2D>("hoodieman");

        am = new(8, 1, 5, 3, new Vector2(192, 192));
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        kState = Keyboard.GetState();
        mState = Mouse.GetState();

        if (Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            posX += 5;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            posX -= 5;
        }

        foreach (ScaledSprite sprite in sprites)
        {
            sprite.Update();
        }

        am.Update();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        foreach (ScaledSprite sprite in sprites)
            _spriteBatch.Draw(sprite.texture, sprite.pos, Color.White);
        _spriteBatch.Draw(spriteSheet, new Rectangle(posX, 100, 192, 192), am.GetFrame(), Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
