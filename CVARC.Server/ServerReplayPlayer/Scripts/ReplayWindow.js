ReplayWindow = {
DELTA_MOVE:	0.03,	DELTA_ANGLE:0.003,
camera:		null,	renderer:	null,	scene:	null,
bodies:		[],		permit:		null,	isEnd:	false,
FPS:		null,	FPSCounter:	0,		FPSTime:0,
startTime:	0,
dX:			0,		dY:			0,		dZ:		0,
preX:		0,		preY:		0,

start:		function() {
	 if(ReplayLoader.isReady())// ждем пока загрузится реплей
		{
		 if(ReplayLoader.hasNext())// проверим, что пришедший реплей не пуст
			{
			 ReplayWindow.permit = ReplayLoader.getNext();
			 ReplayWindow.render();
			}
		}
	 else
		setTimeout(ReplayWindow.start, 100);
},

render: function () {
	 var currentTime = Date.now();
	 if(ReplayWindow.startTime == 0)
		{
		 ReplayWindow.startTime = currentTime;
		 ReplayWindow.FPSTime = currentTime;
		}
	 var time = currentTime - ReplayWindow.startTime;
	 ReplayWindow.doMove();
	 if(ReplayWindow.isEnd == false)
		ReplayWindow.doPermit(time);
//	 ReplayWindow.doFPS(currentTime);
	 ReplayWindow.renderer.render(ReplayWindow.scene, ReplayWindow.camera);
	 requestAnimationFrame(ReplayWindow.render);

},
doMove:		function() {
	 ReplayWindow.bodies[0].rotation.z	-= ReplayWindow.dX * ReplayWindow.DELTA_ANGLE;
	 var x = ReplayWindow.bodies[0].rotation.x - ReplayWindow.dY * ReplayWindow.DELTA_ANGLE;
	 if(x>0)x=0;else if(x<-1.5)x=-1.5;
	 ReplayWindow.bodies[0].rotation.x = x;
	 var z = ReplayWindow.camera.position.z - ReplayWindow.dZ * ReplayWindow.DELTA_MOVE;
	 if(z<50)z=50;else if(z>500)z=500;
	 ReplayWindow.camera.position.z = z;
	 ReplayWindow.dX = 0;
	 ReplayWindow.dY = 0;
	 ReplayWindow.dZ = 0;
},
doPermit:	function(time) {
	 while(ReplayWindow.permit[0] <= time)
		{
		 ReplayWindow.permit[1].forEach(ReplayWindow.process);
		 if(ReplayLoader.hasNext())
			ReplayWindow.permit = ReplayLoader.getNext();
		 else
			{
			 ReplayWindow.isEnd = true;
			 break;
			}
		}
},
process:	function(elem) {
    var body;
	 switch(elem.name)
		{
		 case undefined:
			 body = ReplayWindow.bodies[1*elem.id];	break;
	     case "Box":
		    body = ReplayWindow.addBox(elem);		break;
		 case "Cylinder":
			 body = ReplayWindow.addCylinder(elem);		break;
		 case "Ball":
			 body = ReplayWindow.addBall(elem);		break;
		 default:
			 body = ReplayWindow.addBody(elem);
		}

	 if(elem.x)
		 body.position.x = parseFloat(elem.x);
	 if(elem.y)
		 body.position.y = parseFloat(elem.y);
	 if(elem.z)
		 body.position.z = parseFloat(elem.z);

    //выпилить дурость полная
	 if (elem.name == "Cylinder" || elem.name == undefined) {
	     body.rotation.x = 1.5;
	     if (elem.Z)
	         body.rotation.y = parseFloat(elem.Z);
	 }
//	 if (elem.Z)
//	     body.rotation.z = parseFloat(elem.Z);
//	 if (elem.Y)
//	     body.rotation.y = parseFloat(elem.Y);
//	 if (elem.X)
//	     body.rotation.x = parseFloat(elem.X);
},
addBox:		function(elem) {
	 var geometry = new THREE.CubeGeometry(1*elem.xSize, 1*elem.ySize, 1*elem.zSize);
	 var material;
	 if(elem.texture)
		{
		 var texture = THREE.ImageUtils.loadTexture( '/other/' + elem.texture );
		 material = new THREE.MeshPhongMaterial( {color: elem.color, map: texture} );
		}
	 else
		 material = new THREE.MeshPhongMaterial( {color: elem.color} );
	 var body = new THREE.Mesh( geometry, material );
	 ReplayWindow.bodies[1*elem.id] = body;
	 ReplayWindow.bodies[1*elem.parent].add(body);
	 return body;
},
addCylinder: function (elem) {
    elem.color = elem.id == "1" ? "red" : "blue";
	 var radiusSegments = 5*elem.rTop + 5*elem.rBottom;
	 var geometry = new THREE.CylinderGeometry(1*elem.rTop, 1*elem.rBottom, 1*elem.height, radiusSegments);
	 var material = new THREE.MeshLambertMaterial({ color: elem.color, side: THREE.DoubleSide });
	 var body = new THREE.Mesh(geometry, material);
    
	 var geometry2 = new THREE.CubeGeometry(10, 10, 10);
	 var material2 = new THREE.MeshLambertMaterial({ map: THREE.ImageUtils.loadTexture('/Other/' + elem.color + '.png')});
	 var body2 = new THREE.Mesh(geometry2, material2);
     body.add(body2);
	 body2.position.y += 6;
    
	 ReplayWindow.bodies[1*elem.id] = body;
	 ReplayWindow.bodies[1*elem.parent].add(body);
	 return body;
},
addBall:	function(elem) {
	 var geometry = new THREE.SphereGeometry(1*elem.radius);
	 var material = new THREE.MeshPhongMaterial( {color: elem.color} );
	 var body = new THREE.Mesh( geometry, material );
	 ReplayWindow.bodies[1*elem.id] = body;
	 ReplayWindow.bodies[1*elem.parent].add(body);
	 return body;
},
addBody:	function(elem) {
	 var body = new THREE.Object3D();
	 ReplayWindow.bodies[1*elem.id] = body;
	 ReplayWindow.bodies[1*elem.parent].add(body);
	 return body;
},
doFPS:		function(time) {
	 var dt = time - ReplayWindow.FPSTime;
	 ++ReplayWindow.FPSCounter;

	 if(dt > 166)
		{
		 if(ReplayWindow.FPS)
			 ReplayWindow.FPS.innerText = Math.round( 1000 * ReplayWindow.FPSCounter /  dt);
		 ReplayWindow.FPSTime += dt;
		 ReplayWindow.FPSCounter = 0;
		}
},
addScene: function (div) {
    var width = window.outerWidth;
    var height = window.outerHeight;
	 var camera = new THREE.PerspectiveCamera(75, width/height, 0.1, 1000);
	 var renderer = new THREE.WebGLRenderer({ antialias: true });
	 var scene = new THREE.Scene();
	 var light1 = new THREE.PointLight( 0xffffff, 1.5, 1000 );
	 var light2 = new THREE.PointLight( 0xffffff, 1.5, 1000 );
	 var rootBody = new THREE.Object3D();
	 
	 renderer.setSize( width, height );
	 scene.add(rootBody);
	 light1.position.set(0,-300, 300 );
	 light2.position.set(0, 300, 300 );
	 scene.add( light1 );
	 scene.add( light2 );
	 
	 ReplayWindow.bodies[0] = rootBody;
	 camera.position.z = 283;

	 rootBody.rotation.x = -1.3;
	 rootBody.rotation.z = -0.5;

	 ReplayWindow.bodies[0].rotation.x = -0.72;
	 ReplayWindow.bodies[0].rotation.y = 0;
	 ReplayWindow.bodies[0].rotation.z = -0.004;

	 ReplayWindow.camera = camera;
	 ReplayWindow.renderer = renderer;
	 ReplayWindow.scene = scene;
	 div.appendChild( renderer.domElement );
},

addFPS:		function(div) {
	 var span = document.createElement('span');
	 span.id = 'FPS';
	 span.innerText = '0';
	 div.appendChild(span);
	 ReplayWindow.FPS = span;
},
removeFPS:	function() {
	 ReplayWindow.FPS.parentNode.removeChild(ReplayWindow.FPS);
	 ReplayWindow.FPS = null;
},

addEvents:	function(div) {
	 div.onmousedown = ReplayWindow.mouseDown;
	 div.onmousewheel = ReplayWindow.mouseWheel; // chrome
	 div.addEventListener('DOMMouseScroll', ReplayWindow.mouseWheel); // firefox
},
mouseDown:	function(e) {
	 ReplayWindow.dX = 0;
	 ReplayWindow.dY = 0;
	 ReplayWindow.preX = e.clientX;
	 ReplayWindow.preY = e.clientY;
	 window.onmouseup = ReplayWindow.mouseUp;
	 window.onmousemove = ReplayWindow.mouseMove;
},
mouseWheel:	function(e) {
	 if(e.detail) // firefox
		ReplayWindow.dZ -= e.detail * 40;
	 else if(e.wheelDeltaY) // chrome
		ReplayWindow.dZ += e.wheelDeltaY;
	 e.preventDefault();
},
mouseUp:	function(e) {
	 window.onmouseup = null;
	 window.onmousemove = null;
},
mouseMove:	function(e) {
	 var ddX = ReplayWindow.preX - e.clientX;
	 var ddY = ReplayWindow.preY - e.clientY;
	 
	 ReplayWindow.preX -= ddX;
	 ReplayWindow.preY -= ddY;
	 
	 ReplayWindow.dX += ddX;
	 ReplayWindow.dY += ddY;
}
};