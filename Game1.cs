/* Game1.cs
* BrickBreaker
* Revision History
* Liam Conn, 2024.11.22: Created
* Liam Conn, 2024.11.24: Added Code
* Liam Conn, 2024.11.27  Added Code
* Liam Conn, 2024.11.08: Added Code
* Liam Conn, 2024.12.08: Debugging complete
* Liam Conn, 2024.12.08: Comments added
*
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BrickBreaker
{
    /// <summary>
    /// Main entry point for Game, Handles game wide components, scenes, transitions
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;

        // declare scenes
        private StartScene _startScene;
        private PlayScene _playScene;
        private AboutScene _aboutScene;
        private HelpScene _helpScene;
        private ScoreScene _scoreScene;

        // music fields
        private Song _menuMusic;
        private Song _playMusic;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 740;
            _graphics.PreferredBackBufferHeight = 480;
            SharedScene.sharedStage = new Vector2(_graphics.PreferredBackBufferWidth, 
                _graphics.PreferredBackBufferHeight);

            base.Initialize();
        }

        /// <summary>
        /// Load all game assets fonts, textures, init game scenes
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load music files
            _menuMusic = Content.Load<Song>("audio/menusong");
            _playMusic = Content.Load<Song>("audio/gamesong");

            // Init scenes
            _startScene = new StartScene(this);
            _playScene = new PlayScene(this);
            _aboutScene = new AboutScene(this);
            _helpScene = new HelpScene(this);
            _scoreScene = new ScoreScene(this);
            // Add scenes to Components list
            this.Components.Add(_startScene);
            this.Components.Add(_playScene);
            this.Components.Add(_aboutScene);
            this.Components.Add(_helpScene);
            this.Components.Add(_scoreScene);

            _startScene.ShowScene();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            int selectedOption = 0;
            KeyboardState kState = Keyboard.GetState();
            // if on starting screen
            if(_startScene.Enabled)
            {
                // make sure song isn't already playing
                if(MediaPlayer.Queue.ActiveSong != _menuMusic)
                {
                    MediaPlayer.Volume = 0.5f;
                    MediaPlayer.Stop(); // stop any current music
                    MediaPlayer.Play(_menuMusic); // start menu music
                    MediaPlayer.IsRepeating = true;
                }
                selectedOption = _startScene.Menu.SelectedOption;
                // options to select from and hiding/showing scenes for each
                if(selectedOption == 0 && kState.IsKeyDown(Keys.Enter))
                {
                    _startScene.HideScene();

                    // start gameplay music
                    MediaPlayer.Volume = 0.1f;
                    MediaPlayer.Stop();
                    MediaPlayer.Play(_playMusic);
                    MediaPlayer.IsRepeating = true;

                    _playScene.ResetGamePlay();
                    _playScene.ShowScene();
                } 
                else if(selectedOption == 1 && kState.IsKeyDown(Keys.Enter))
                {
                    _startScene.HideScene();
                    _scoreScene.ShowScene();
                }
                else if(selectedOption == 2 && kState.IsKeyDown(Keys.Enter))
                {
                    _startScene.HideScene();
                    _helpScene.ShowScene();
                }
                else if(selectedOption == 3 && kState.IsKeyDown(Keys.Enter))
                {
                    _startScene.HideScene();
                    _aboutScene.ShowScene();
                }
                else if (selectedOption == 4 && kState.IsKeyDown(Keys.Enter))
                {
                    Exit();
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}
