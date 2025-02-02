/* PlayScene.cs
* BrickBreaker
* Revision History
* Liam Conn, 2024.11.22: Created
* Liam Conn, 2024.11.24: Added Code
* Liam Conn, 2024.11.27  Added Code
* Liam Conn, 2024.11.06: Added Code
* Liam Conn, 2024.12.08: Debugging complete
* Liam Conn, 2024.12.08: Comments added
*
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace BrickBreaker
{
    /// <summary>
    /// Main Gameplay Scene where player can interact with ball,
    /// paddle, and bricks, as well as view their score
    /// </summary>
    public class PlayScene : GameScene
    {
        // PlayScene Game Entities
        private Paddle _paddle;
        private Ball _ball;
        private List<Brick> _bricks;
        private CollisionManager _collisionManager;

        // Scoring variables
        private int _playerScore;
        private SpriteFont _scoreFont;
        private Vector2 _scorePosition;

        // List of Player Scores for ScoreScene
        public static List<(string playerName, int score)> PlayerScores = 
            new List<(string playerName, int score)>();

        // Game over variables
        private bool _isGameOver;
        private string _gameOverMsg;
        private Rectangle _menuButton;
        private Rectangle _exitButton;
        private MouseState _previousMouseState;
        private Texture2D buttonTexture;

        // Sound Effects
        private SoundEffect _ballBounce;
        private SoundEffect _brickHit;

        /// <summary>
        /// Get new instance of PlayScene class
        /// </summary>
        /// <param name="game"></param>
        public PlayScene(Game game) : base(game)
        {
            Game1 game1 = (Game1)game;
            SpriteBatch sbPlay = game1._spriteBatch;

            // Load Sound effects
            _ballBounce = game1.Content.Load<SoundEffect>("audio/ballbounce");
            _brickHit = game1.Content.Load<SoundEffect>("audio/brickbreak");

            // Load textures
            Texture2D paddleTexture = game1.Content.Load<Texture2D>("images/paddle");
            Texture2D ballTexture = game1.Content.Load<Texture2D>("images/ball");
            Texture2D brickTexture = game1.Content.Load<Texture2D>("images/BricksAtlas");
            buttonTexture = game1.Content.Load<Texture2D>("images/button");
            
            // Load/initialize score display and position at top-right
            _scoreFont = game1.Content.Load<SpriteFont>("fonts/ScoreFont");
            _playerScore = 0;
            _scorePosition = new Vector2(SharedScene.sharedStage.X - 200, 10);


            // game over variable intialization
            _isGameOver = false;
            _gameOverMsg = "";

            _menuButton = new Rectangle((int)(SharedScene.sharedStage.X / 2 - 100), 300, 200, 50); //button size and position
            _exitButton = new Rectangle((int)(SharedScene.sharedStage.X / 2 - 100), 400, 200, 50);

            // Initialize paddle and ball
            _paddle = new Paddle(game, sbPlay, paddleTexture);
            _ball = new Ball(game, sbPlay, ballTexture);

            // add ball event to GameOverEvent method
            _ball.OnBallMiss += GameOverEvent;

            // Initialize bricks
            _bricks = new List<Brick>();
            BuildBrickGrid();

            // Add components to game
            Components.Add(_paddle);
            Components.Add(_ball);

            // Initialize CollisionManager
            _collisionManager = new CollisionManager(game, this, _ball, _paddle, _bricks,
                _ballBounce, _brickHit);
            Components.Add(_collisionManager);
        }

        /// <summary>
        /// Update PlayScene logic, checks for game over state and
        /// updates components if necessary 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if(_isGameOver)
            {
                MouseState currentMs = Mouse.GetState();

                // check for menu button click
                if(_menuButton.Contains(currentMs.Position) &&
                    currentMs.LeftButton == ButtonState.Pressed &&
                    _previousMouseState.LeftButton == ButtonState.Released)
                {
                    // Reset PlayScene
                    ResetGamePlay();

                    // Return to main menu
                    Game1 game1 = (Game1)Game;
                    this.HideScene();
                    game1.Components.OfType<StartScene>().FirstOrDefault()?.ShowScene();
                }

                // check for exit button click
                if(_exitButton.Contains(currentMs.Position) &&
                    currentMs.LeftButton == ButtonState.Pressed &&
                    _previousMouseState.LeftButton == ButtonState.Released)
                {
                    Game.Exit(); // close game
                }
                // track last mouse click
                _previousMouseState = currentMs;

            } else
            {
                // game win (cleared all bricks)
                if(_bricks.TrueForAll(brick => !brick.IsVisible))
                {
                    GameOverEvent();
                }

                // continue to update game scene as usual
                base.Update(gameTime);
            }
            
        }

        /// <summary>
        /// Draw some PlayScene elements, handle game over display
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            Game1 game1 = (Game1)Game;
            SpriteBatch sb = game1._spriteBatch;

            sb.Begin();

            if(_isGameOver)
            {
                // Draws game over msg
                sb.DrawString(_scoreFont, _gameOverMsg, 
                    new Vector2(SharedScene.sharedStage.X/2 -150, 200), Color.White);

                // draw menu button
                sb.Draw(buttonTexture, _menuButton, Color.White);
                sb.DrawString(_scoreFont, "Main Menu", 
                    new Vector2(_menuButton.X + 40, _menuButton.Y + 15), Color.White);

                // draw exit button
                sb.Draw(buttonTexture, _exitButton, Color.White);
                sb.DrawString(_scoreFont, "Exit", 
                    new Vector2(_exitButton.X + 70, _exitButton.Y + 15), Color.White);

            } else
            {
                // Draw game elements
                base.Draw(gameTime);
                // Draws the score onto the screen over top of the other existing elements
                string scoreTxt = $"Game Score: {_playerScore}";
                sb.DrawString(_scoreFont, scoreTxt, _scorePosition, Color.White);
            }

            sb.End();  
        }

        /// <summary>
        /// Creates a grid of bricks at start of game/reset of game
        /// </summary>
        private void BuildBrickGrid()
        {
            Game1 game1 = (Game1)Game;
            SpriteBatch sbPlay = game1._spriteBatch;

            // Clear any existing bricks
            _bricks.Clear();

            // Number of rows, columns, max health
            int numCols = 10;
            int numRows = 5;
            int maxHealth = 4;

            int brickWidth = 64;
            int brickHeight = 32;
            int padding = 5;

            int totalBrickWidth = numCols * (brickWidth + padding) - padding;
            int startX = (int)((SharedScene.sharedStage.X - totalBrickWidth) / 2);

            // loop for creating brick rows/cols
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    Vector2 position = new Vector2(startX + col * (brickWidth + padding),
                                                   50 + row * (brickHeight + padding));

                    Brick brick = new Brick(Game, sbPlay, 
                        game1.Content.Load<Texture2D>("images/BricksAtlas"), position, maxHealth);
                    _bricks.Add(brick);
                }
            }

            // Add bricks to components
            Components.RemoveAll(c => c is Brick); // Remove any old bricks from components
            foreach (Brick brick in _bricks)
            {
                Components.Add(brick);
            }
        }

        /// <summary>
        /// Add points to player score
        /// </summary>
        /// <param name="awardPoints">Points to be added to score</param>
        public void AddScore(int awardPoints)
        {
            _playerScore += awardPoints;
        }

        /// <summary>
        /// Handle game over logic, add player score to leaderboard
        /// </summary>
        private void GameOverEvent()
        {
            _isGameOver = true;
            _gameOverMsg = $"Game Over! Your Final Score: {_playerScore}";
            // Add player score to list
            PlayerScores.Add(($"Player {PlayerScores.Count + 1}", _playerScore));
        }

        /// <summary>
        /// Reset game elements, ball, paddle, brick grid, etc
        /// for a new game.
        /// </summary>
        public void ResetGamePlay()
        {
            // Reset game over vars
            _isGameOver=false;
            _gameOverMsg = "";
            _playerScore = 0;

            // Reset Ball/Paddle to starting position
            _ball.ResetPosition();
            _paddle.ResetPosition();

            // Reset bricks
            BuildBrickGrid();

        }
    }
}
