using NUnit.Framework;
namespace Movement { 
[TestFixture] public partial class Round {
[Test] public void Forward() { TestRunner.RunDemo("Movement_Round_Forward"); }}}
namespace Movement { 
[TestFixture] public partial class Round {
[Test] public void Backward() { TestRunner.RunDemo("Movement_Round_Backward"); }}}
namespace Movement { 
[TestFixture] public partial class Rect {
[Test] public void Forward() { TestRunner.RunDemo("Movement_Rect_Forward"); }}}
namespace Movement { 
[TestFixture] public partial class Rect {
[Test] public void Backward() { TestRunner.RunDemo("Movement_Rect_Backward"); }}}
namespace Movement { 
[TestFixture] public partial class Rect {
[Test] public void Square() { TestRunner.RunDemo("Movement_Rect_Square"); }}}
namespace Movement { 
[TestFixture] public partial class Round {
[Test] public void Square() { TestRunner.RunDemo("Movement_Round_Square"); }}}
namespace Movement { 
[TestFixture] public partial class Rect {
[Test] public void Rotate() { TestRunner.RunDemo("Movement_Rect_Rotate"); }}}
namespace Movement { 
[TestFixture] public partial class Round {
[Test] public void Rotate() { TestRunner.RunDemo("Movement_Round_Rotate"); }}}
namespace Movement { 
[TestFixture] public partial class Limit {
[Test] public void Linear() { TestRunner.RunDemo("Movement_Limit_Linear"); }}}
namespace Movement { 
[TestFixture] public partial class Limit {
[Test] public void Round() { TestRunner.RunDemo("Movement_Limit_Round"); }}}
namespace Movement { 
[TestFixture] public partial class Limit {
[Test] public void Round2() { TestRunner.RunDemo("Movement_Limit_Round2"); }}}
namespace Interaction { 
[TestFixture] public partial class Rect {
[Test] public void Alignment() { TestRunner.RunDemo("Interaction_Rect_Alignment"); }}}
namespace Interaction { 
[TestFixture] public partial class Round {
[Test] public void Alignment() { TestRunner.RunDemo("Interaction_Round_Alignment"); }}}
namespace Interaction { 
[TestFixture] public partial class Round {
[Test] public void Alignment2() { TestRunner.RunDemo("Interaction_Round_Alignment2"); }}}
namespace Interaction { 
[TestFixture] public partial class Rect {
[Test] public void Collision() { TestRunner.RunDemo("Interaction_Rect_Collision"); }}}
namespace Interaction { 
[TestFixture] public partial class Rect {
[Test] public void BrickCollision() { TestRunner.RunDemo("Interaction_Rect_BrickCollision"); }}}
namespace Gripping { 
[TestFixture] public partial class Rect {
[Test] public void Grip() { TestRunner.RunDemo("Gripping_Rect_Grip"); }}}
namespace Gripping { 
[TestFixture] public partial class Round {
[Test] public void Grip() { TestRunner.RunDemo("Gripping_Round_Grip"); }}}
namespace Gripping { 
[TestFixture] public partial class Round {
[Test] public void GripAndMove() { TestRunner.RunDemo("Gripping_Round_GripAndMove"); }}}
namespace Gripping { 
[TestFixture] public partial class Rect {
[Test] public void GripAndMove() { TestRunner.RunDemo("Gripping_Rect_GripAndMove"); }}}
namespace Gripping { 
[TestFixture] public partial class Rect {
[Test] public void GripAndMove2() { TestRunner.RunDemo("Gripping_Rect_GripAndMove2"); }}}
namespace Gripping { 
[TestFixture] public partial class Rect {
[Test] public void GripThroughWall() { TestRunner.RunDemo("Gripping_Rect_GripThroughWall"); }}}
namespace Gripping { 
[TestFixture] public partial class Rect {
[Test] public void Release() { TestRunner.RunDemo("Gripping_Rect_Release"); }}}
namespace Gripping { 
[TestFixture] public partial class Rect {
[Test] public void GripUnGripable() { TestRunner.RunDemo("Gripping_Rect_GripUnGripable"); }}}
namespace Gripping { 
[TestFixture] public partial class Rect {
[Test] public void GripFromBack() { TestRunner.RunDemo("Gripping_Rect_GripFromBack"); }}}
namespace Collision { 
[TestFixture] public partial class Rect {
[Test] public void NoCollision() { TestRunner.RunDemo("Collision_Rect_NoCollision"); }}}
namespace Collision { 
[TestFixture] public partial class Rect {
[Test] public void CollisionCount() { TestRunner.RunDemo("Collision_Rect_CollisionCount"); }}}
namespace Collision { 
[TestFixture] public partial class Rect {
[Test] public void CollisionBox() { TestRunner.RunDemo("Collision_Rect_CollisionBox"); }}}
namespace Collision { 
[TestFixture] public partial class Rect {
[Test] public void NoBox() { TestRunner.RunDemo("Collision_Rect_NoBox"); }}}
namespace Collision { 
[TestFixture] public partial class Rect {
[Test] public void RotateNoBox() { TestRunner.RunDemo("Collision_Rect_RotateNoBox"); }}}
namespace Collision { 
[TestFixture] public partial class Rect {
[Test] public void RotateBox() { TestRunner.RunDemo("Collision_Rect_RotateBox"); }}}
namespace Collision { 
[TestFixture] public partial class Rect {
[Test] public void WallCollisionTime() { TestRunner.RunDemo("Collision_Rect_WallCollisionTime"); }}}
namespace Collision { 
[TestFixture] public partial class Rect {
[Test] public void PushingWithBrick() { TestRunner.RunDemo("Collision_Rect_PushingWithBrick"); }}}
namespace Collision { 
[TestFixture] public partial class Rect {
[Test] public void RotateWallCollision() { TestRunner.RunDemo("Collision_Rect_RotateWallCollision"); }}}
namespace Collision { 
[TestFixture] public partial class Rect {
[Test] public void BrickCollision() { TestRunner.RunDemo("Collision_Rect_BrickCollision"); }}}
namespace Collision { 
[TestFixture] public partial class Rect {
[Test] public void WallAndBrickCollision() { TestRunner.RunDemo("Collision_Rect_WallAndBrickCollision"); }}}
namespace DWM { 
[TestFixture] public partial class Movement {
[Test] public void LongForward() { TestRunner.RunDWM("DWM_Movement_LongForward"); }}}
namespace DWM { 
[TestFixture] public partial class Movement {
[Test] public void Forward() { TestRunner.RunDWM("DWM_Movement_Forward"); }}}
namespace DWM { 
[TestFixture] public partial class Movement {
[Test] public void Backward() { TestRunner.RunDWM("DWM_Movement_Backward"); }}}
namespace DWM { 
[TestFixture] public partial class Movement {
[Test] public void RightRotate() { TestRunner.RunDWM("DWM_Movement_RightRotate"); }}}
namespace DWM { 
[TestFixture] public partial class Movement {
[Test] public void LeftRotate() { TestRunner.RunDWM("DWM_Movement_LeftRotate"); }}}
namespace DWM { 
[TestFixture] public partial class Movement {
[Test] public void ArcRight() { TestRunner.RunDWM("DWM_Movement_ArcRight"); }}}
namespace DWM { 
[TestFixture] public partial class Movement {
[Test] public void ArcLeft() { TestRunner.RunDWM("DWM_Movement_ArcLeft"); }}}
namespace DWM { 
[TestFixture] public partial class Movement {
[Test] public void Turning() { TestRunner.RunDWM("DWM_Movement_Turning"); }}}
namespace DWM { 
[TestFixture] public partial class Movement {
[Test] public void ArcMoving() { TestRunner.RunDWM("DWM_Movement_ArcMoving"); }}}
[TestFixture] public partial class Encoder {
[Test] public void MoveForward() { TestRunner.RunDWM("Encoder_MoveForward"); }}
[TestFixture] public partial class Encoder {
[Test] public void ArcMoving() { TestRunner.RunDWM("Encoder_ArcMoving"); }}
[TestFixture] public partial class GAX {
[Test] public void CircleMoving() { TestRunner.RunDWM("GAX_CircleMoving"); }}
[TestFixture] public partial class GAX {
[Test] public void Rotating() { TestRunner.RunDWM("GAX_Rotating"); }}
