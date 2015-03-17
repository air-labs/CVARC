using NUnit.Framework;

#region MovementTests
namespace Movement { 
[TestFixture] public partial class Round {
[Test] public void Forward() { TestRunner.Run("Movement_Round_Forward"); }}}
namespace Movement { 
[TestFixture] public partial class Round {
[Test] public void Backward() { TestRunner.Run("Movement_Round_Backward"); }}}
namespace Movement { 
[TestFixture] public partial class Rect {
[Test] public void Forward() { TestRunner.Run("Movement_Rect_Forward"); }}}
namespace Movement { 
[TestFixture] public partial class Rect {
[Test] public void Backward() { TestRunner.Run("Movement_Rect_Backward"); }}}
namespace Movement { 
[TestFixture] public partial class Rect {
[Test] public void Square() { TestRunner.Run("Movement_Rect_Square"); }}}
namespace Movement { 
[TestFixture] public partial class Round {
[Test] public void Square() { TestRunner.Run("Movement_Round_Square"); }}}
namespace Movement { 
[TestFixture] public partial class Rect {
[Test] public void Rotate() { TestRunner.Run("Movement_Rect_Rotate"); }}}
namespace Movement { 
[TestFixture] public partial class Round {
[Test] public void Rotate() { TestRunner.Run("Movement_Round_Rotate"); }}}
namespace Movement { 
[TestFixture] public partial class Limit {
[Test] public void Linear() { TestRunner.Run("Movement_Limit_Linear"); }}}
namespace Movement { 
[TestFixture] public partial class Limit {
[Test] public void Round() { TestRunner.Run("Movement_Limit_Round"); }}}
namespace Movement {
[TestFixture]
public partial class Limit {
[Test] public void Linear2() { TestRunner.Run("Movement_Limit_Linear2"); }}}
namespace Movement {
[TestFixture]
public partial class Limit {
[Test] public void Round2() { TestRunner.Run("Movement_Limit_Round2"); }}}
#endregion

#region InteractionTests
namespace Interaction { 
[TestFixture] public partial class Rect {
[Test] public void Alignment() { TestRunner.Run("Interaction_Rect_Alignment"); }}}
namespace Interaction { 
[TestFixture] public partial class Rect {
[Test] public void Collision() { TestRunner.Run("Interaction_Rect_Collision"); }}}
namespace Interaction { 
[TestFixture] public partial class Round {
[Test] public void Alignment() { TestRunner.Run("Interaction_Round_Alignment"); }}}
namespace Interaction {
[TestFixture]
public partial class Round {
[Test] public void Alignment2() { TestRunner.Run("Interaction_Round_Alignment2"); }}}
#endregion

#region GrippingTests
namespace Gripping {
[TestFixture] public partial class Rect {
[Test] public void Grip() { TestRunner.Run("Gripping_Rect_Grip"); }}}
namespace Gripping {
[TestFixture] public partial class Rect {
[Test] public void GripThroughWall() { TestRunner.Run("Gripping_Rect_GripThroughWall"); }}}
namespace Gripping {
[TestFixture] public partial class Rect {
[Test] public void Release() { TestRunner.Run("Gripping_Rect_Release"); }}}
namespace Gripping {
[TestFixture] public partial class Rect {
[Test] public void GripUnGripable() { TestRunner.Run("Gripping_Rect_GripUnGripable"); }}}
namespace Gripping {
[TestFixture]
public partial class Rect {
[Test] public void BackGripping() { TestRunner.Run("Gripping_Rect_GripFromBack"); }}}
#endregion

#region Collision
namespace Collision {
[TestFixture]
public partial class Rect {
[Test] public void NoCollision() { TestRunner.Run("Collision_Rect_NoCollision"); }}}
namespace Collision {
[TestFixture]
public partial class Rect {
[Test] public void CollisionCount() { TestRunner.Run("Collision_Rect_CollisionCount"); }}}
namespace Collision {
[TestFixture]
public partial class Rect {
[Test] public void CollisionBox() { TestRunner.Run("Collision_Rect_CollisionBox"); }}}
namespace Collision {
[TestFixture]
public partial class Rect {
[Test] public void NoBox() { TestRunner.Run("Collision_Rect_NoBox"); }}}
namespace Collision {
[TestFixture]
public partial class Rect {
[Test] public void RotateNoBox() { TestRunner.Run("Collision_Rect_RotateNoBox"); }}}
namespace Collision {
[TestFixture]
public partial class Rect {
[Test] public void RotateBox() { TestRunner.Run("Collision_Rect_RotateBox"); }}}
#endregion

#region CameraTests
#endregion