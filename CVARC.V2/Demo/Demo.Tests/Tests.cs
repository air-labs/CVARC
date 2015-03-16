using NUnit.Framework;
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
[Test] public void Linear2() { TestRunner.Run("Movement_Limit_-Linear"); }}}
namespace Movement {
[TestFixture]
public partial class Limit {
[Test] public void Round2() { TestRunner.Run("Movement_Limit_-Round"); }}}
namespace Movement {
[TestFixture]
public partial class Rect {
[Test] public void SpeedAndGripTest() { TestRunner.Run("Movement_Rect_Strange"); }}}
namespace Interaction { 
[TestFixture] public partial class Rect {
[Test] public void Alignment() { TestRunner.Run("Interaction_Rect_Alignment"); }}}
namespace Interaction { 
[TestFixture] public partial class Rect {
[Test] public void Collision() { TestRunner.Run("Interaction_Rect_Collision"); }}}
namespace Interaction {
[TestFixture] public partial class Rect {
[Test] public void Grip() { TestRunner.Run("Gripping_Rect_Grip"); }}}
namespace Interaction{
[TestFixture] public partial class Rect {
[Test] public void GripThroughWall() { TestRunner.Run("Gripping_Rect_GripThroughWall"); }}}
namespace Interaction{
[TestFixture] public partial class Rect {
[Test] public void Release() { TestRunner.Run("Gripping_Rect_Release"); }}}
namespace Interaction {
[TestFixture] public partial class Rect {
[Test] public void GripUnGripable() { TestRunner.Run("Gripping_Rect_GripUnGripable"); }}}
namespace Interaction { 
[TestFixture] public partial class Round {
[Test] public void Alignment() { TestRunner.Run("Interaction_Round_Alignment"); }}}
namespace Interaction {
[TestFixture]
public partial class Round {
[Test] public void Alignment2() { TestRunner.Run("Interaction_Round_Alignment2"); }}}
namespace Interaction {
[TestFixture]
public partial class Rect {
[Test] public void BackGripping() { TestRunner.Run("Gripping_Rect_GripFromBack"); }}}