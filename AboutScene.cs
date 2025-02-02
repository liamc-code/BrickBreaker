/* AboutScene.cs
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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickBreaker
{
    /// <summary>
    /// About Scene, displays info about game/developer
    /// </summary>
    public class AboutScene : GameScene
    {
        // Font related variables
        private SpriteBatch _aboutSb;
        private SpriteFont _bodyFont;
        private string[] _aboutText;

        /// <summary>
        /// New instance of AboutScene class
        /// </summary>
        /// <param name="game"></param>
        public AboutScene(Game game) : base(game)
        {
            Game1 game1 = (Game1)game;
            _aboutSb = game1._spriteBatch;
            // load font
            _bodyFont = game1.Content.Load<SpriteFont>("fonts/Body");

            // about text as string array
            _aboutText = new string[]
            {
                "Brick Breaker Game",
                "Developed by: Liam Conn",
                "Version 1.0",
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

            // Go back to main menu if Backspace is pressed
            if(kState.IsKeyDown(Keys.Back))
            {
                this.HideScene();
                Game1 game1 = (Game1)Game;
                game1.Components.OfType<StartScene>().FirstOrDefault()?.ShowScene();
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Draw About Scene to screen
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            _aboutSb.Begin();
            // position of text
            Vector2 position = new Vector2(SharedScene.sharedStage.X / 2 - 350, SharedScene.sharedStage.Y / 2);

            // for each line of text
            foreach (string txtLine in _aboutText)
            {
                _aboutSb.DrawString(_bodyFont, txtLine, position, Color.White);
                position.Y += _bodyFont.LineSpacing;
            }

            _aboutSb.End();
            base.Draw(gameTime);
        }
    }
}
