angular.module('starter.services', [])

.service('LoginService', function($q) {
    return {
        loginUser: function(name, pw) {
            var deferred = $q.defer();
            var promise = deferred.promise;
 
            if (name.localeCompare("driver") == 0 && pw.localeCompare("pass") == 0) {
                deferred.resolve('Welcome ' + name + '!');
            } else {
                deferred.reject('Wrong credentials.');
            }
            promise.success = function(fn) {
                promise.then(fn);
                return promise;
            }
            promise.error = function(fn) {
                promise.then(null, fn);
                return promise;
            }
            return promise;
        }
    }
})

.service('sharedActivity', function () {
	var activity = "";
	var cabCode = "";
	var routeName = "";
    var username = "";

	return {
		getActivity: function () {
			return activity;
		},
		setActivity: function(value) {
			activity = value;
		},
		getCab: function () {
			return cabCode;
		},
		setCab: function(value) {
			cabCode = value;
		},
		getRoute: function () {
			return routeName;
		},
		setRoute: function(value) {
			routeName = value;
		},
        getUsername: function () {
			return username;
		},
		setUsername: function(value) {
			username = value;
		}
	};
})

.service('speedTracker', function () {
	var speed = 0;
	var latitude = 0;
	var longitude = 0;
	var timeStamp = -1;
	
	return {
		getSpeed: function () {
			return speed;
		},
		setSpeed: function(value) {
			speed = value;
		},
		updateSpeed: function(newLatitude, newLongitude, newTimeStamp) {
			if(timeStamp != -1 && newTimeStamp > timeStamp) {
				var longDiff = Math.abs(newLongitude - longitude);
				var latDiff = Math.abs(newLatitude - latitude);
				var timeDiff = Math.abs(newTimeStamp - timeStamp)/(1000*60*60); // in hours
				var distanceDiff = Math.sqrt(longDiff*longDiff + latDiff*latDiff)
				speed = distanceDiff/timeDiff;
			}
			latitude = newLatitude;
			longitude = newLongitude;
			timeStamp = newTimeStamp;
		},
		getLongitude: function () {
			return longitude;
		},
		setLongitude: function(value) {
			longitude = value;
		},
		getLatitude: function () {
			return latitude;
		},
		setLatitude: function(value) {
			latitude = value;
		},
		getTimeStamp: function () {
			return latitude;
		},
		setTimeStamp: function(value) {
			timeStamp = value;
		}
	};
})

.factory('Chats', function() {
  // Might use a resource here that returns a JSON array

  // Some fake testing data
  var chats = [{
    id: 0,
    name: 'Ben Sparrow',
    lastText: 'You on your way?',
    face: 'https://pbs.twimg.com/profile_images/514549811765211136/9SgAuHeY.png'
  }, {
    id: 1,
    name: 'Max Lynx',
    lastText: 'Hey, it\'s me',
    face: 'https://avatars3.githubusercontent.com/u/11214?v=3&s=460'
  }, {
    id: 2,
    name: 'Andrew Jostlin',
    lastText: 'Did you get the ice cream?',
    face: 'https://pbs.twimg.com/profile_images/491274378181488640/Tti0fFVJ.jpeg'
  }, {
    id: 3,
    name: 'Adam Bradleyson',
    lastText: 'I should buy a boat',
    face: 'https://pbs.twimg.com/profile_images/479090794058379264/84TKj_qa.jpeg'
  }, {
    id: 4,
    name: 'Perry Governor',
    lastText: 'Look at my mukluks!',
    face: 'https://pbs.twimg.com/profile_images/491995398135767040/ie2Z_V6e.jpeg'
  }];

  return {
    all: function() {
      return chats;
    },
    remove: function(chat) {
      chats.splice(chats.indexOf(chat), 1);
    },
    get: function(chatId) {
      for (var i = 0; i < chats.length; i++) {
        if (chats[i].id === parseInt(chatId)) {
          return chats[i];
        }
      }
      return null;
    }
  };
});
