/* HelpScene.cs
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
    /// Help Scene, provides instructions on how to play game
    /// </summary>
    public class HelpScene : GameScene
    {
        // variables to display help text
        private SpriteBatch _helpSb;
        private SpriteFont _bodyFont;
        private string[] _helpText;

        /// <summary>
        /// New instance of HelpScene class
        /// </summary>
        /// <param name="game"></param>
        public HelpScene(Game game) : base(game)
        {
            Game1 game1 = (Game1)game;
            _helpSb = game1._spriteBatch;
            // load texture for help text
            _bodyFont = game1.Content.Load<SpriteFont>("fonts/Body");

            // help instructions as string array
            _helpText = new string[]
            {
                "How to Play:",
                "1. Use Left/Right arrow keys to move the paddle.",
                "2. Bounce the ball to break bricks.",
                "3. Clear all the bricks to win.",
                "4. Keep your ball from hitting the ground.",
                "",
                "Press Backspace to return to Main Menu"
            };
        }

        /// <summary>
        /// Update override for Key event to back out of scene,
        /// go back to Menu Screen
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            KeyboardState kState = Keyboard.GetState();

            // go back to main menu if backspace pressed
            if (kState.IsKeyDown(Keys.Back))
            {
                this.HideScene();
                Game1 game1 = (Game1)Game;
                game1.Components.OfType<StartScene>().FirstOrDefault()?.ShowScene();
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Draw help instructions on screen
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            _helpSb.Begin();
            // position of text
            Vector2 position = new Vector2(SharedScene.sharedStage.X / 2 - 350, SharedScene.sharedStage.Y / 2 - 100);
            foreach(string txtLine in _helpText) // draw each line onto screen
            {
                _helpSb.DrawString(_bodyFont, txtLine, position, Color.White);
                position.Y += _bodyFont.LineSpacing;
            }
            _helpSb.End();
            base.Draw(gameTime);
        }
    }
}
