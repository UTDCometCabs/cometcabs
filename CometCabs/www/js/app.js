// Ionic Starter App

// angular.module is a global place for creating, registering and retrieving Angular modules
// 'starter' is the name of this angular module example (also set in a <body> attribute in index.html)
// the 2nd parameter is an array of 'requires'
angular.module('starter', ['ionic'])

.controller('MapCtrl', function($scope, $ionicLoading, $compile) {
      function initialize() {
		/*Latitude and longitude for the school. Don't know whether we could use more precision */
        var site = new google.maps.LatLng(32.986,-96.750);
 
      
        var mapOptions = {
          streetViewControl:true,
          center: site,
          zoom: 16, /*This can be played with. It's either +-1 that we can see building shapes */
//          mapTypeId: google.maps.MapTypeId.ROADMAP /*This is the default type, so we don't need this line.
        };
		/*The element references the div id in index.html*/
        var map = new google.maps.Map(document.getElementById("map"),
            mapOptions);
        

		/*Part of the example code I started from. Apparently unnecessary*/
        //$scope.map = map;
		/*Code to add a marker representing a single icon to represent a cab.
		The icon does not resize, so I'll have to add some sort of zoom listener
		Will probably use http://stackoverflow.com/questions/3281524/resize-markers-depending-on-zoom-google-maps-v3
		as a reference for that*/
			var marker = new google.maps.Marker({
		map: map,
		position: new google.maps.LatLng(32.986498, -96.751010),
		icon: {
		path: google.maps.SymbolPath.BACKWARD_CLOSED_ARROW,
		fillOpacity: 1.0,
		fillColor: '005763',
		strokeOpacity: 1.0,
		strokeColor: 'fff000',
		strokeWeight: 1.0,				
		scale: 8 //pixels
		//map: map
		}		
		}); 
		var rutfordCoords = [
			new google.maps.LatLng(32.980504, -96.750881),
			new google.maps.LatLng(32.98422, -96.751053),
			new google.maps.LatLng(32.992023, -96.750967),
			new google.maps.LatLng(32.992068, -96.752984)
			];
		var rutfordRoute = new google.maps.Polyline({
			path: rutfordCoords,
			geodesic: true, 
			strokeColor: 'fff000',
			strokeOpacity: 1.0,
			strokeWeight: 2.0		
		});
		rutfordRoute.setMap(map);
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
