/* ScoreScene.cs
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickBreaker
{
    /// <summary>
    /// Display scoreboard with top 5 scores
    /// </summary>
    public class ScoreScene : GameScene
    {
        // variables for displaying scores
        private SpriteBatch _scoreSb;
        private SpriteFont _titleFont;
        private SpriteFont _scoreFont;

        private Vector2 _scorePosition;
        // list to store user scores/names
        private List<(string playerName, int score)> _scores;

        /// <summary>
        /// New instance of ScoreScene class
        /// </summary>
        /// <param name="game"></param>
        public ScoreScene(Game game) : base(game)
        {
            Game1 game1 = (Game1)game;
            _scoreSb = game1._spriteBatch;
            // Load fonts for displaying the scores
            _titleFont = game1.Content.Load<SpriteFont>("fonts/NormalFont");
            _scoreFont = game1.Content.Load<SpriteFont>("fonts/ScoreFont");

            // starting position for score title
            _scorePosition = new Vector2(SharedScene.sharedStage.X / 2 - 100, 50);

            // uses scores from PlayScene
            _scores = PlayScene.PlayerScores;
        }

        /// <summary>
        /// Update override for Key event to back out of scene,
        /// go back to Menu Screen
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            KeyboardState kState = Keyboard.GetState();

            // Go back to main menu if Backspace is pressed
            if (kState.IsKeyDown(Keys.Back))
            {
                this.HideScene();
                Game1 game1 = (Game1)Game;
                game1.Components.OfType<StartScene>().FirstOrDefault()?.ShowScene();
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Draw top 5 scores to screen
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            _scoreSb.Begin();

            // Draw title
            _scoreSb.DrawString(_titleFont, "Top 5 Scores", _scorePosition, Color.Red);

            // each player score
            for(int i = 0; i < _scores.Count && i < 5; i++)
            {
                // score text
                string scoreTxt = $"{_scores[i].playerName} : {_scores[i].score}";
                // Draw scores
                Vector2 playerPosition = new Vector2(SharedScene.sharedStage.X / 2 - 150, 150 + (i * 20));
                _scoreSb.DrawString(_scoreFont, scoreTxt, playerPosition, Color.White);
            }

            _scoreSb.End();
            base.Draw(gameTime);
        }

    }
}
