(function () {
    'use strict';

    // Factory name is handy for logging
    var serviceId = 'vimeoPlayerSvc';

    // Define the factory on the module.
    // Inject the dependencies. 
    // Point to the factory definition function.
    // TODO: replace app with your module name
    angular.module('app').factory(serviceId, ['$rootScope', vimeoPlayerSvc]);

    function vimeoPlayerSvc($rootScope) {
        // Define the functions and properties to reveal.
        var service = {
            play: play,
            pause: pause,
            unload: unload,
        };

        return service;

        function play() {
            $rootScope.$broadcast("vimeo-play");
        }

        function pause() {
            $rootScope.$broadcast("vimeo-pause");
        }

        function unload() {
            $rootScope.$broadcast("vimeo-unload");
        }

        //#region Internal Methods        

        //#endregion
    }
})();