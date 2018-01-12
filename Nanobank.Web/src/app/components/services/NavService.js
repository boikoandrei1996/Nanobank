(function(){
  'use strict';

  angular.module('app')
          .service('navService', [
          '$q',
          navService
  ]);

  function navService($q){
    var menuItems = [
      {
        name: 'Список заявок',
        icon: 'dashboard',
        sref: '.dashboard',
        roles : ['admin', 'user'],
      },
      {
        name: 'Мои сделки',
        icon: 'view_module',
        sref: '.table',
        roles : ['admin', 'user'],
      },
      {
        name: 'Личный кабинет',
        icon: 'person',
        sref: '.profile',
        roles : ['admin', 'user'],
      },
      {
        name: 'Оформить заявку',
        icon: 'create',
        sref: '.newdeal',
        roles : ['admin', 'user'],
      },
      {
        name: 'Пользователи',
        icon: 'supervisor_account',
        sref: '.users',
        roles : ['admin'],
      },
      {
        name: 'Сделки',
        icon: 'shopping_cart',
        sref: '.requests',
        roles : ['admin'],
      },
      {
        name: 'Жалобы',
        icon: 'thumb_down',
        sref: '.complains',
        roles : ['admin'],
      }
    ];

    return {
      loadUserItems : function(){
        return $q.when(menuItems.filter(function(item){
          return item.roles.includes('user');
        }))
      },
      loadAllItems : function() {
        return $q.when(menuItems);
      }
    };
  }

})();
