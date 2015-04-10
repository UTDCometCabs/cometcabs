// Ionic Starter App

// angular.module is a global place for creating, registering and retrieving Angular modules
// 'starter' is the name of this angular module example (also set in a <body> attribute in index.html)
// the 2nd parameter is an array of 'requires'
angular.module('starter', ['ionic'])

.controller('MapCtrl', function($scope, $ionicLoading, $compile) {
	var rutfordCoords;
	var phase1SouthCoords;
	var phase1NorthCoords;
	var commonsCoords;
	var mcDermottCoords;
	
	var rutfordRoute;		
	var phase1SouthRoute;	
	var phase1NorthRoute;
	var commonsRoute;	
	var mcDermottRoute;
		
	var rutfordMarker; 
	var phase1SouthMarker;
	
	var map;
	function initialize() {
		/*Latitude and longitude for the school. Don't know whether we could use more precision */
        var site = new google.maps.LatLng(32.986,-96.750);
		
      
        var mapOptions = {
          streetViewControl:true,
          center: site,
          zoom: 16, /*This can be played with. It's either +-1 that we can see building shapes */
        };
		/*The element references the div id in index.html*/
        map = new google.maps.Map(document.getElementById("map"),
            mapOptions);
        setRouteCoordinates(); //Sets the details for routes
		setCabMarkers();
		
		/*Sets a marker for the current position */
		if (navigator.geolocation) {
			navigator.geolocation.getCurrentPosition(function(position) {
			var currentPosition = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
			var marker = new google.maps.Marker({
				position: currentPosition,
				map: map,
				title:"You are here"
				});		
			});
		} else {
			alert("Geolocation is not supported by this browser.");
		}
		
		google.maps.event.addListener(map, 'zoom_changed', function() {
     if (map.getZoom() < 16) map.setZoom(16);
   });
       
  }
  
  function setRouteCoordinates() {
	/*
		The JSON for routes:
		var jsonRoute = {
			"name": <string>,
			"color": <in the form #000000>,
			[
			{"latitude": <double>, "longitude": <double> } ]
		}	
			With two or more points in the array.	
	*/
	
	var routes = [
		{
			"name": "McDermott",
			"color": '#003986',
			"path": [
				{"latitude": 32.990034, "longitude": -96.744273},
				{"latitude": 32.989341, "longitude": -96.745367},
				{"latitude": 32.988540, "longitude": -96.745046},
				{"latitude": 32.987919, "longitude": -96.746194},
				{"latitude": 32.987982, "longitude": -96.746966},
				{"latitude": 32.987361, "longitude": -96.747020},
				{"latitude": 32.987487, "longitude": -96.746172},
				{"latitude": 32.984653, "longitude": -96.745990},
				{"latitude": 32.984635, "longitude": -96.745367},
				{"latitude": 32.985616, "longitude": -96.745325},
				{"latitude": 32.985616, "longitude": -96.745958},
				{"latitude": 32.987829, "longitude": -96.746204},
				{"latitude": 32.988405, "longitude": -96.744938},			
				{"latitude": 32.988747, "longitude": -96.745046},			
				{"latitude": 32.989359, "longitude": -96.743908},		
				{"latitude": 32.990034, "longitude": -96.744273}			
			]
		},
		{
			"name": "Commons",
			"color": '#9978c8',
			"path": [
				{"latitude":32.985645, "longitude": -96.74914},		
				{"latitude":32.985666, "longitude": -96.750987},
				{"latitude":32.990643, "longitude": -96.750946},
				{"latitude":32.990721, "longitude": -96.753669},
				{"latitude":32.991789, "longitude": -96.753631},
				{"latitude":32.991789, "longitude": -96.752607},
				{"latitude":32.990734, "longitude": -96.752620},
				{"latitude":32.990721, "longitude": -96.750949},
				{"latitude":32.990643, "longitude": -96.750946}				
			]
		},
		{
			"name": "Phase 1 North",
			"color": '#FF9900',
			"path": [
				{"latitude":32.985645, "longitude":-96.74914},			
				{"latitude":32.985668, "longitude":-96.754610},
				{"latitude":32.985850, "longitude":-96.754610},
				{"latitude":32.985864, "longitude":-96.753907},
				{"latitude":32.986955, "longitude":-96.753751},
				{"latitude":32.987130, "longitude":-96.753778},
				{"latitude":32.987124, "longitude":-96.753910},
				{"latitude":32.988195, "longitude":-96.753947},
				{"latitude":32.988219, "longitude":-96.755001},
				{"latitude":32.988388, "longitude":-96.755001},
				{"latitude":32.988368, "longitude":-96.753725},
				{"latitude":32.985668, "longitude":-96.753741}			
			]
		},
		{
			"name": "Phase 1 South",
			"color": '#FF33FF',
			"path": [
				{"latitude":32.985647, "longitude":-96.74914},			
				{"latitude":32.985672, "longitude":-96.754277},
				{"latitude":32.985456, "longitude":-96.754309},
				{"latitude":32.985035, "longitude":-96.753988},
				{"latitude":32.983748, "longitude":-96.754399},
				{"latitude":32.983750, "longitude":-96.755866},
				{"latitude":32.985345, "longitude":-96.755054},
				{"latitude":32.985664, "longitude":-96.755254},
				{"latitude":32.985672, "longitude":-96.754277}			
			]
		},
		{
			"name": "Rutford Rd",
			"color": '#005710',
			"path": [
				{"latitude":32.981273, "longitude":-96.750881},
				{"latitude":32.981273, "longitude":-96.750190},
				{"latitude":32.980547, "longitude":-96.750190},
				{"latitude":32.980504, "longitude":-96.750881},
				{"latitude":32.98422, "longitude":-96.751053},
				{"latitude":32.992023, "longitude":-96.750967},
				{"latitude":32.992068, "longitude":-96.752984},
				{"latitude":32.992768, "longitude":-96.753057},
				{"latitude":32.992757, "longitude":-96.751475},
				{"latitude":32.992068, "longitude":-96.751461}	
			]
		}
	];

		
	for (i = 0; i < routes.length; i++) {
		setRouteFromJSON(routes[i]);
	}
	
  }
  function setRouteFromJSON(route) {
	var routePath = [];
	for (j = 0; j < route.path.length; j++) {
		routePath.push(new google.maps.LatLng(route.path[j].latitude, route.path[j].longitude));
	}
	var routeLine = new google.maps.Polyline({
		path: routePath,
		geodesic: true,
		strokeColor: route.color,
		strokeOpacity: 1.0,
		strokeWeight: 2.5,
		map: map
	});
	
  }


  function setCabColor(isFull, onDuty) {
	var color = "#008000" ;
	if (isFull) {
		color = "#FFFF00";
	}
	if (!onDuty) {
		color = "#FF0000";
	}
	return color;
  }
  
  function setCabMarkers() {
  		/*Code to add a marker representing a single icon to represent a cab.
		The icon does not resize, so I'll have to add some sort of zoom listener
		Will probably use http://stackoverflow.com/questions/3281524/resize-markers-depending-on-zoom-google-maps-v3
		as a reference for that*/
	
	/* The JSON for these should be:
		var jsonCab = {
			"latitude": <double>
			"longitude": <double>
			"isFull": <true or false>
			"onDuty": <true or false>		
		}
		
	*/
	var cabs = [
		{
			"latitude": 32.986498,
			"longitude": -96.751010,
			"isFull": true,
			"onDuty": true
		},
		{
			"latitude": 32.985672,
			"longitude": -96.754399,
			"isFull": false,
			"onDuty": false
		}	
	
	];
	
	for (i = 0; i < cabs.length; i++) {
		drawCab(cabs[i].latitude, cabs[i].longitude, cabs[i].isFull, cabs[i].onDuty);
	}

  }
  
  function drawCab(latitude, longitude, isFull, onDuty) {
	cab = new google.maps.Marker({
		map: map,
		position: new google.maps.LatLng(latitude, longitude),
		icon: {
		path: google.maps.SymbolPath.BACKWARD_CLOSED_ARROW,
		fillOpacity: 1.0,
		fillColor: setCabColor(isFull, onDuty),
		strokeOpacity: 1.0,
		strokeColor: setCabColor(isFull, onDuty),
		strokeWeight: 1.0,				
		scale: 7 //pixels
		}		
		});   
  }

	  /*Last line of code from my things*/
      google.maps.event.addDomListener(window, 'load', initialize);
	/*Everything after here was from example code (http://paulsutherland.net/ionic-and-google-maps-api/. 
		I don't know how these affect the app*/
      $scope.centerOnMe = function() {
        if(!$scope.map) {
          return;
        }

        $scope.loading = $ionicLoading.show({
          content: 'Getting current location...',
          showBackdrop: false
        });
        navigator.geolocation.getCurrentPosition(function(pos) {
          $scope.map.setCenter(new google.maps.LatLng(pos.coords.latitude, pos.coords.longitude));
          $scope.loading.hide();
        }, function(error) {
          alert('Unable to get location: ' + error.message);
        });
      };
      
      $scope.clickTest = function() {
        alert('Example of infowindow with ng-click')
      };
      
    });

