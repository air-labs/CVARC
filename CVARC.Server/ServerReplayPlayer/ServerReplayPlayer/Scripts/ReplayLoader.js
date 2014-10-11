ReplayLoader = {
dt:		0,	size:	0,	pos:	0,	delayTime:	0,
data:	[],	types: ["Body", "Ball", "Box", "Cylinder"],
order:	["x", "y", "z", "Z", "Y", "X", "parent", "color"],
typesOrder: [[], ["radius"], ["xSize", "ySize", "zSize", "texture"], ["rTop", "rBottom", "height"]],
ready:	false,

load:	function(url, replay) {
	 var req = ReplayLoader.getXmlHttp();
	 req.onreadystatechange = ReplayLoader.stateChange;
	 req.open("GET", url, true);
	 req.send(null);
	 //ReplayLoader.data = [["0,1,0,0,0,0,0,0,0,#FFFFFF","-3,2,45,60,0,0,0,1.570796326794896619,0,#5500BB,35,35,150","-3,3,0,0,0,0,0,1.570796326794896619,1,#55BBBB,35,35,150"]];
	 //ReplayLoader.size = ReplayLoader.data.length;
	 //ReplayLoader.dt = 0.016666666666666666;
	 //ReplayLoader.ready=true;
},
setDelayTime:	function(time) {
	 ReplayLoader.delayTime = Math.round( time*1000 );
},

isReady:	function() { 
	 return ReplayLoader.ready;
},

getNext:	function() {
	 var pos = ReplayLoader.pos++;
	 var time = Math.round( (pos-1)*ReplayLoader.dt ) - ReplayLoader.delayTime;

	 var array = ReplayLoader.data[pos].map(ReplayLoader.convert);
	 
	 return [time, array];
},
convert:	function(str) {
	 var array = str.split(",");
	 var type = array.shift();
	 var object = {};
	 
	 if(type > 0)
		{
		 object.id = type;
		 ReplayLoader.readEdit(object, array);
		}
		else
		{
		 object.id = array.shift();
		 ReplayLoader.readEdit(object, array);
		 ReplayLoader.readCreate(object, array, -type);
		}
	 return object;
},
readEdit:	function(object, array) {
	 var order = ReplayLoader.order;
	 for(var i in order)
		{
		 var elem = array.shift();
		 if(elem && elem != "")
			object[order[i]] = elem;
		}
},
readCreate:	function(object, array, type) {
	 var order = ReplayLoader.typesOrder[type];
	 object.name = ReplayLoader.types[type];
	 for(var i in order)
		{
		 var elem = array.shift();
		 if(elem && elem != "")
			object[order[i]] = elem;
		}
},
hasNext:	function() {
	 return ReplayLoader.pos < ReplayLoader.size;
},

stateChange: function(e) {
	 if(e.target.readyState == 4 && e.target.status == 200)
		{
		 ReplayLoader.data = JSON.parse(e.target.responseText);
		 ReplayLoader.dt = ReplayLoader.data[ReplayLoader.pos++] * 1000;
		 ReplayLoader.size = ReplayLoader.data.length;
		 ReplayLoader.ready = true;
		}
},
getXmlHttp:	function() {
	 var xmlhttp;
	 try{
		 xmlhttp = new ActiveXObject("Msxml2.XMLHTTP");
		} catch (e)
		{
		 try{
			 xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
			} catch (E) {
			 xmlhttp = false;
			}
		}
	 if (!xmlhttp && typeof XMLHttpRequest!='undefined')
		 xmlhttp = new XMLHttpRequest();
	 return xmlhttp;
}
};