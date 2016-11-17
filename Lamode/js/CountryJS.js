$(document).ready(function () {
    $('#Country').change(function () {
        $.ajax({
            url: '/Home/StatesByCountry',
            type: 'POST',
            data: { countryId: $(this).val() },
            datatype: 'json',
            success: function (data) {
                var options = '';
                $.each(data, function () {
                    options += '<option value="' + this.id + '">' + this.state + '</option>';
                });
                $('#State').prop('disabled', false).html(options);
            }
        });
    });
});