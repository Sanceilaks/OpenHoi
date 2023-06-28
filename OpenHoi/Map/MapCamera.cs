using Microsoft.Xna.Framework;

namespace OpenHoi.Map;

public class MapCamera : GameComponent
{
	public Vector2 Position { get; set; }
	public float Rotation { get; set; }
	public Vector2 Scale { get; set; }
	public float Zoom { get; set; }
	public Matrix ViewMatrix { get; set; } = Matrix.Identity;
	public Matrix ProjectionMatrix { get; set; } = Matrix.Identity;
	public Vector3 UpVector { get; set; } = Vector3.Up;
	public MapCamera(OpenHoi game) : base(game)
	{
		
	}
}