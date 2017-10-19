$('input[type="checkbox"]')
    .change(function() {
        var text = '';
        if ($(this).is(":checked")) {

            $(this)
                .parent()
                .find('img')
                .css({
                    'background-color': "white",
                    'opacity': "0.5"
                });


        } else if ($(this).not(":checked")) {
            $(this)
                .parent()
                .find('img')
                .css({
                    'opacity': "1"
                });
        }
    });