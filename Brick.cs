/* Brick.cs
* BrickBreaker
* Revision History
* Liam Conn, 2024.11.22: Created
* Liam Conn, 2024.11.24: Added Code
* Liam Conn, 2024.11.08: Added Code
* Liam Conn, 2024.12.08: Debugging complete
* Liam Conn, 2024.12.08: Comments added
*
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BrickBreaker
{
    /// <summary>
    /// Single brick in game, health and texture states
    /// </summary>
    public class Brick : DrawableGameComponent
    {
        // brick properties
        private SpriteBatch _sbBrick;
        private Texture2D _brickTexture;
        private Vector2 _brickPosition;
        private Rectangle _sourceRectangle;

        // brick health
        private int _health;
        private int _maxHealth;

        // Width/height of brick in atlas
        private const int SourceWidth = 383;  
        private const int SourceHeight = 130;
        
        // width/height of brick on screen
        private const int TargetWidth = 64;   
        private const int TargetHeight = 32;

        // visibility of brick
        public bool IsVisible { get; set; }

        /// <summary>
        /// New instance of brick class
        /// </summary>
        /// <param name="game"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="maxHealth"></param>
        public Brick(Game game, SpriteBatch spriteBatch, Texture2D texture, Vector2 position, int maxHealth)
            : base(game)
        {
            _sbBrick = spriteBatch;
            _brickTexture = texture;
            _brickPosition = position;

            _maxHealth = maxHealth;
            _health = maxHealth;

            IsVisible = true;
            UpdateSourceRectangle();
        }

        /// <summary>
        /// Texture maps from atlas corresponding to specific brick images
        /// </summary>
        private static readonly (int row, int col)[] HealthToTextureMap = new (int, int)[]
        {
            (3, 2), // Health 8: Solid Dark Blue
            (0, 0), // Health 7: Cracked Dark Blue
            (1, 1), // Health 6: Solid Dark Green
            (0, 1), // Health 5: Cracked Dark Green
            (1, 0), // Health 4: Solid Lime Green
            (2, 0), // Health 3: Cracked Lime Green
            (3, 1), // Health 2: Solid Yellow
            (2, 1)  // Health 1: Cracked Yellow
        };

        /// <summary>
        /// Update texture region based on brick health
        /// </summary>
        private void UpdateSourceRectangle()
        {
            // Reverse health to access the textures properly
            int currentHealth = _maxHealth - (_health - 1);

            if (currentHealth > 0 && currentHealth <= HealthToTextureMap.Length)
            {
                var (row, col) = HealthToTextureMap[currentHealth - 1];

                // includes x/y offsets for atlas and width and height of the texture
                _sourceRectangle = new Rectangle(
                    col * SourceWidth, 
                    row * SourceHeight, 
                    SourceWidth,        
                    SourceHeight
                );
            }
            else
            {
                IsVisible = false; // Hide the brick when health runs out
            }
        }

        /// <summary>
        /// Reduce brick health and update visibility
        /// </summary>
        public void TakeHit()
        {
            if (_health > 0)
            {
                _health--;
                UpdateSourceRectangle();

                if (_health <= 0) // brick destroyed
                {
                    IsVisible = false;
                }
            }
        }

        /// <summary>
        /// Draw brick if visible
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (IsVisible)
            {
                // Create brick rectangle
                Rectangle destinationRectangle = new Rectangle(
                    (int)_brickPosition.X,
                    (int)_brickPosition.Y,
                    TargetWidth,   // Scaled width
                    TargetHeight   // Scaled height
                );
                // Draw brick
                _sbBrick.Draw(
                    _brickTexture,          
                    destinationRectangle,   
                    _sourceRectangle,       
                    Color.White             
                );
            }
            base.Draw(gameTime);
        }

        /// <summary>
        /// Get the bounding rectangle for collision detection
        /// </summary>
        /// <returns></returns>
        public Rectangle GetBounds()
        {
            if (IsVisible) // only if its still in game
            {
                return new Rectangle(
                    (int)_brickPosition.X,
                    (int)_brickPosition.Y,
                    TargetWidth,
                    TargetHeight
                );
            }
            return Rectangle.Empty;
        }

        /// <summary>
        /// Reset brick to original state
        /// </summary>
        public void Reset()
        {
            IsVisible = true;
            _health = _maxHealth;
            UpdateSourceRectangle();
        }
    }
}
