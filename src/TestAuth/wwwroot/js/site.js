// Write your Javascript code.
(function () {
    angular.module('SimpleRESTWebsite', ['LocalStorageModule', 'ui.router'])
        .constant('ENDPOINT_URI', 'http://localhost:9551/')
        .config(function($stateProvider, $urlRouterProvider, $httpProvider) {
                $stateProvider
                    .state('login', {
                        url: '/login',
                        templateUrl: 'app/templates/login.tmpl.html',
                        controller: 'LoginCtrl',
                        controllerAs: 'login'
                    })
                    .state('dashboard', {
                        url: '/dashboard',
                        templateUrl: 'app/templates/dashboard.tmpl.html'
                    });;

                $urlRouterProvider.otherwise('/dashboard');

            $httpProvider.interceptors.push('APIInterceptor');
        })
        .service('APIInterceptor', function ($rootScope) {
            var service = this;

            service.responseError = function (response) {
                if (response.status === 401) {
                    $rootScope.$broadcast('unauthorized');
                }
                return response;
            };
        })
        .service('UserService', function (localStorageService) {
           var service = this,
               currentUser = null;

           service.setCurrentUser = function (user) {
               currentUser = user;
               localStorageService.set('user', user);
               return currentUser;
           };

           service.getCurrentUser = function () {
               if (!currentUser) {
                   currentUser = localStorageService.get('user');
               }
               return currentUser;
           };
       })
        .service('LoginService', function ($http, ENDPOINT_URI) {
            var service = this,
                path = 'Account/';

            function getUrl() {
                return ENDPOINT_URI + path;
            }

            function getLogUrl(action) {
                return getUrl() + action;
            }

            service.login = function (credentials) {
                return $http.post(getLogUrl('Login'), credentials);
            };

            service.logout = function () {
                return $http.post(getLogUrl('LogOff'));
            };

            service.register = function (user) {
                return $http.post(getLogUrl('Register'), user);
            };
        })
        .controller('LoginCtrl', function ($rootScope, $state, LoginService, UserService) {
            var login = this;

            function signIn(user) {
                LoginService.login(user)
                    .then(function (response) {
                        //user.access_token = response.data.id;
                        UserService.setCurrentUser(user);
                        $rootScope.$broadcast('authorized');
                        $state.go('dashboard');
                    });
            }

            function register(user) {
                debugger;
                user.confirmPassword = user.password;
                LoginService.register(user)
                    .then(function (response) {
                        if (response.status === 200)
                            signIn(user);
                        else {
                            console.log(response);
                        }
                    });
            }

            function submit(user) {
                login.newUser ? register(user) : signIn(user);
            }

            login.newUser = false;
            login.submit = submit;
        })
        .controller('MainCtrl', function ($rootScope, $state, LoginService, UserService) {
            var main = this;

            function logout() {
                LoginService.logout()
                    .then(function (response) {
                        main.currentUser = UserService.setCurrentUser(null);
                        $state.go('login');
                    }, function (error) {
                        console.log(error);
                    });
            }

            $rootScope.$on('authorized', function () {
                main.currentUser = UserService.getCurrentUser();
            });

            $rootScope.$on('unauthorized', function () {
                main.currentUser = UserService.setCurrentUser(null);
                $state.go('login');
            });

            main.logout = logout;
            main.currentUser = UserService.getCurrentUser();
        })
        ;
})();
