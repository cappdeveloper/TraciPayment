var challengerApp = new function () {
    return {
        DeleteData: function (e) {

            $this = $(e.target);

            bootbox.confirm("Are you sure do you want to delete this record?<br/>Note: Deleted data cannot be restored.", function (confirmed) {
                if (confirmed) {

                    $.ajax({
                        type: "Post",
                        async: true,
                        url: $this.attr("href"),
                        datatype: "json",
                        data: {
                            id: $this.attr("id")
                        },
                        beforeSend: function () {
                            $.blockUI({ message: '<img src="/Images/loader2.gif" />' });
                        },
                        success: function (data) {
                            if (data.success == true) {
                                $this.closest('tr').remove();
                                bootbox.alert('Record has successfully deleted.');
                            } else {
                                bootbox.alert('<p style="color:red;">' + data.message + '</p>');
                            }
                        },
                        complete: function () {
                            $.unblockUI();
                        },
                        error: function (xhr, textStatus, errorThrown) {
                            $.unblockUI();
                            bootbox.alert(xhr + textStatus + errorThrown);
                        }
                    })
                }
            });
        },
        ActiveData: function (e) {

            $this = $(e.target);

            bootbox.confirm("Are you sure do you want to Make Active/Inactive Process?<br/>", function (confirmed) {
                if (confirmed) {

                    $.ajax({
                        type: "Post",
                        async: true,
                        url: $this.attr("href"),
                        datatype: "json",
                        data: {
                            id: $this.attr("id")
                        },
                        beforeSend: function () {
                            $.blockUI({ message: '<img src="/Images/loader2.gif" />' });
                        },
                        success: function (data) {
                            if (data.success == true) {
                                bootbox.alert('Record has successfully Updated.');
                                window.location.reload();

                            } else {
                                bootbox.alert('<p style="color:red;">' + data.message + '</p>');
                            }
                        },
                        complete: function () {
                            $.unblockUI();
                        },
                        error: function (xhr, textStatus, errorThrown) {
                            $.unblockUI();
                            bootbox.alert(xhr + textStatus + errorThrown);
                        }
                    })
                }
            });
        },

        UpdateProfile: function (param) {

            $.ajax({
                type: "Post",
                async: true,
                url: '/MyProfile/UpdateProfile',
                contentType: "application/json; charset=utf-8",
                datatype: "json",
                data: JSON.stringify(param),
                beforeSend: function () {
                    $.blockUI({ message: '<img src="/Images/loader2.gif" />' });
                },
                success: function (data) {
                    if (data.success == true) {
                        bootbox.alert('Changes on your profile has been saved!');
                    } else {
                        bootbox.alert('<p style="color:red;">An error occured while processing your request! Please try again.</p>');
                    }
                },
                complete: function () {
                    $.unblockUI();
                },
                error: function (xhr, textStatus, errorThrown) {
                    $.unblockUI();
                    bootbox.alert(xhr + textStatus + errorThrown);
                }
            })
        },
        CreateEdit: function (form, successCallback) {
            var $this = form;

            if (jQuery($this).validationEngine('validate')) {
                $.ajax({
                    type: "POST",
                    async: true,
                    url: $this.attr('action'),
                    datatype: "json",
                    data: $($this).serialize(),
                    beforeSend: function () {
                        $.blockUI({ message: '<img src="/Images/loader2.gif" />' });
                    },
                    success: function (data) {

                        successCallback(data);
                    },
                    complete: function () {

                        $.unblockUI();
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        $.unblockUI();
                        alert(xhr, textStatus, errorThrown);
                        console.log(errorThrown);
                    }
                });
            }
        },
        GetList: function (url, id, successCallback) {
            $.ajax({
                type: "GET",
                async: true,
                url: url,
                datatype: "text/html",
                data: {
                    Id: id
                },
                beforeSend: function () {
                    $.blockUI({ message: '<img src="/Images/loader2.gif" />' });
                },
                success: function (data) {
                    successCallback(data);
                },
                complete: function () {

                    $.unblockUI();
                },
                error: function (xhr, textStatus, errorThrown) {
                    $.unblockUI();
                    alert(xhr, textStatus, errorThrown);

                }
            });
        },
        GetListNoParam: function (url,successCallback) {
            $.ajax({
                type: "GET",
                async: true,
                url: url,
                datatype: "text/html",
                data: {
                },
                beforeSend: function () {
                    $.blockUI({ message: '<img src="/Images/loader2.gif" />' });
                },
                success: function (data) {
                    successCallback(data);
                },
                complete: function () {

                    $.unblockUI();
                },
                error: function (xhr, textStatus, errorThrown) {
                    $.unblockUI();
                    alert(xhr, textStatus, errorThrown);

                }
            });
        }
    };

}

$('.table .delete-link').live('click',function (e) {
    e.preventDefault();
    challengerApp.DeleteData(e);
    return false;
})



$('#update-profile-button').click(function (e) {
    e.preventDefault();
    var name = $('#Name').val();
    var param = {
        name: name
    }
    challengerApp.UpdateProfile(param);
    return false;
})





