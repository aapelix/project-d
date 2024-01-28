using System.Collections.Generic;
using System.Diagnostics;
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

    SpriteEffects s = SpriteEffects.FlipHorizontally;

    int posX;
    // int posY;

    int animation = 0;
    int animationFrames = 2;
    int animationDuration = 10;
    string animationState = "Idle";

    bool isRunning;
    bool isIdle = true;

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

        newAnimation();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        kState = Keyboard.GetState();
        mState = Mouse.GetState();

        if (!isRunning && Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            animationState = "Running";
            isRunning = true;
            isIdle = false;
        }

        if (!isRunning && Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            animationState = "Running";
            isRunning = true;
            isIdle = false;
        }

        if (Keyboard.GetState().IsKeyUp(Keys.Left) && Keyboard.GetState().IsKeyUp(Keys.Right) && isIdle == false)
        {
            animationState = "Idle";
            isIdle = true;
            isRunning = false;
        }

        switch (animationState)
        {
            case "Idle":
                animation = 0;
                animationFrames = 2;
                animationDuration = 10;
                newAnimation();
                break;
            case "Running":
                animation = 3;
                animationFrames = 8;
                animationDuration = 5;
                newAnimation();
                break;
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
        _spriteBatch.Draw(spriteSheet, new Rectangle(posX, 100, 192, 192), am.GetFrame(), Color.White, 0, new Vector2(0, 0), s, 0);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void newAnimation()
    {
        am = new(animationFrames, 1, animationDuration, animation, new Vector2(192, 192));
    }
}
