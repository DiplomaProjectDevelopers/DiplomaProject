$('.btnSubmitRole').click(function () {
    const userId = this.id.substr(10, this.id.length - 10);
    const roleId = $('#roleList' + userId.toString()).val();
    const model = { userId, roleId };
    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: "/Admin/UpdateRole",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            alert(data.message);
        },
        error: function () {
            alert("Error occured!!")
        }  
    });

});         


$(document).ready(function () {
    $("#user-tbl #checkall").click(function () {
        if ($("#user-tbl #checkall").is(':checked')) {
            $("#user-tbl input[type=checkbox]").each(function () {
                $(this).prop("checked", true);
            });

        } else {
            $("#user-tbl input[type=checkbox]").each(function () {
                $(this).prop("checked", false);
            });
        }
    });

    $("[data-toggle=tooltip]").tooltip();
});