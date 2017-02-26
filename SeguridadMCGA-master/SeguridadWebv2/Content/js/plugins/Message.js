function GetCounts(servicename, service, obj)
{
    $.ajax(service, {
        dataType: 'json',
        data: {
            format: 'json'
        },
        success: function (result) {
            obj.html(result);
        },
        error: function () {
            alert('Message Services [' + servicename + ']: An error occurred');
        }
    });
}