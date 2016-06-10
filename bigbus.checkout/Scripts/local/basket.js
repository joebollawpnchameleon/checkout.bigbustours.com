$(document).ready(function () {
  var toggle = $('.js-toggle-basket');
  var basket = $('.basket__container');
  $(toggle).on('click', function(e) {
    e.preventDefault();
    $(toggle).toggleClass('basket__toggle_active');
    $(basket).toggleClass('basket__container_open');
  });
  // $('.form__input-date').datepicker();

  if (!Modernizr.touchevents || !Modernizr.inputtypes.date) {
    $('input[type=date]')
      .attr('type', 'text')
      .datepicker({
        // Consistent format with the HTML5 picker
        dateFormat: 'yy-mm-dd',
        formatDate: 'M',
        nextText: "",
        prevText: ""
    });
  }

});
