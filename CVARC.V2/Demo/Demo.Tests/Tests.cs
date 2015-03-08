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
