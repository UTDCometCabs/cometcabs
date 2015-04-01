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
		setRouteCoordinates();
      
        var mapOptions = {
          streetViewControl:true,
          center: site,
          zoom: 16, /*This can be played with. It's either +-1 that we can see building shapes */
        };
		/*The element references the div id in index.html*/
        map = new google.maps.Map(document.getElementById("map"),
            mapOptions);
        
		setRouteLines();
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
	mcDermottCoords = [
		new google.maps.LatLng(32.990034, -96.744273),			
		new google.maps.LatLng(32.989341, -96.745367),
		new google.maps.LatLng(32.988540, -96.745046),
		new google.maps.LatLng(32.987919, -96.746194),
		new google.maps.LatLng(32.987982, -96.746966),
		new google.maps.LatLng(32.987361, -96.747020),
		new google.maps.LatLng(32.987487, -96.746172),
		new google.maps.LatLng(32.984653, -96.745990),
		new google.maps.LatLng(32.984635, -96.745367),
		new google.maps.LatLng(32.985616, -96.745325),
		new google.maps.LatLng(32.985616, -96.745958),
		new google.maps.LatLng(32.987829, -96.746204),
		new google.maps.LatLng(32.988405, -96.744938),			
		new google.maps.LatLng(32.988747, -96.745046),			
		new google.maps.LatLng(32.989359, -96.743908),		
		new google.maps.LatLng(32.990034, -96.744273)
	];
	commonsCoords = [
		new google.maps.LatLng(32.985645, -96.74914),			
		new google.maps.LatLng(32.985666, -96.750987),
		new google.maps.LatLng(32.990643, -96.750946),
		new google.maps.LatLng(32.990721, -96.753669),
		new google.maps.LatLng(32.991789, -96.753631),
		new google.maps.LatLng(32.991789, -96.752607),
		new google.maps.LatLng(32.990734, -96.752620),
		new google.maps.LatLng(32.990721, -96.750949),
		new google.maps.LatLng(32.990643, -96.750946)			
	];	
	
	phase1NorthCoords = [
		new google.maps.LatLng(32.985645, -96.74914),			
		new google.maps.LatLng(32.985668, -96.754610),
		new google.maps.LatLng(32.985850, -96.754610),
		new google.maps.LatLng(32.985864, -96.753907),
		new google.maps.LatLng(32.986955, -96.753751),
		new google.maps.LatLng(32.987130, -96.753778),
		new google.maps.LatLng(32.987124, -96.753910),
		new google.maps.LatLng(32.988195, -96.753947),
		new google.maps.LatLng(32.988219, -96.755001),
		new google.maps.LatLng(32.988388, -96.755001),
		new google.maps.LatLng(32.988368, -96.753725),
		new google.maps.LatLng(32.985668, -96.753741)		
	];
	
	phase1SouthCoords = [
		new google.maps.LatLng(32.985647, -96.74914),			
		new google.maps.LatLng(32.985672, -96.754277),
		new google.maps.LatLng(32.985456, -96.754309),
		new google.maps.LatLng(32.985035, -96.753988),
		new google.maps.LatLng(32.983748, -96.754399),
		new google.maps.LatLng(32.983750, -96.755866),
		new google.maps.LatLng(32.985345, -96.755054),
		new google.maps.LatLng(32.985664, -96.755254),
		new google.maps.LatLng(32.985672, -96.754277)			
	];
	
	rutfordCoords = [
		new google.maps.LatLng(32.981273, -96.750881),
		new google.maps.LatLng(32.981273, -96.750190),
		new google.maps.LatLng(32.980547, -96.750190),
		new google.maps.LatLng(32.980504, -96.750881),
		new google.maps.LatLng(32.98422, -96.751053),
		new google.maps.LatLng(32.992023, -96.750967),
		new google.maps.LatLng(32.992068, -96.752984),
		new google.maps.LatLng(32.992768, -96.753057),
		new google.maps.LatLng(32.992757, -96.751475),
		new google.maps.LatLng(32.992068, -96.751461)
	];
  }
  
  function setRouteLines() {
	rutfordRoute = new google.maps.Polyline({
		path: rutfordCoords,
		geodesic: true, 
		strokeColor: '#005710',
		strokeOpacity: 1.0,
		strokeWeight: 2.5,
		map: map				
	});
		
	phase1SouthRoute = new google.maps.Polyline({
		path: phase1SouthCoords,
		geodesic: true, 
		strokeColor: '#FF33FF',
		strokeOpacity: 1.0,
		strokeWeight: 2.5,	
		map: map
	});
	
	phase1NorthRoute = new google.maps.Polyline({
		path: phase1NorthCoords,
		geodesic: true, 
		strokeColor: '#FF9900',
		strokeOpacity: 0.5,
		strokeWeight: 2.5,	
		map: map
	});

	commonsRoute = new google.maps.Polyline({
		path: commonsCoords,
		geodesic: true, 
		strokeColor: '#9978c8',
		strokeOpacity: 0.5,
		strokeWeight: 2.5,	
		map: map
	});
	
	mcDermottRoute = new google.maps.Polyline({
		path: mcDermottCoords,
		geodesic: true, 
		strokeColor: '#003986',
		strokeOpacity: 1.0,
		strokeWeight: 2.5,	
		map: map
	});
  }

  function setCabMarkers() {
  		/*Code to add a marker representing a single icon to represent a cab.
		The icon does not resize, so I'll have to add some sort of zoom listener
		Will probably use http://stackoverflow.com/questions/3281524/resize-markers-depending-on-zoom-google-maps-v3
		as a reference for that*/
	rutfordMarker = new google.maps.Marker({
		map: map,
		position: new google.maps.LatLng(32.986498, -96.751010),
		icon: {
		path: google.maps.SymbolPath.BACKWARD_CLOSED_ARROW,
		fillOpacity: 1.0,
		fillColor: '#008000',
		strokeOpacity: 1.0,
		strokeColor: '#008000',
		strokeWeight: 1.0,				
		scale: 7 //pixels
		//map: map
		}		
		}); 

		

	phase1SouthMarker = new google.maps.Marker({
		map: map,
		position: new google.maps.LatLng(32.985672, -96.754399),
		icon: {
		path: google.maps.SymbolPath.BACKWARD_CLOSED_ARROW,
		fillOpacity: 1.0,
		fillColor: '#FF0000',
		strokeOpacity: 1.0,
		strokeColor: '#FF0000',
		strokeWeight: 1.0,	
		rotation: -90,
		scale: 7 //pixels
		//map: map
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
/*The below is in the javascript by default. I left it there, but uncommenting it breaks the code */
	/*
.run(function($ionicPlatform) {
  $ionicPlatform.ready(function() {
    // Hide the accessory bar by default (remove this to show the accessory bar above the keyboard
    // for form inputs)
    if(window.cordova && window.cordova.plugins.Keyboard) {
      cordova.plugins.Keyboard.hideKeyboardAccessoryBar(true);
    }
    if(window.StatusBar) {
      StatusBar.styleDefault();
    }
  });
})*/
