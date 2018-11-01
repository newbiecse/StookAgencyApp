var CustomerManagement = function () {

    var $table, $rowTemplate;

    var toggerActionButtons = function ($tr) {
        $tr.find('.editting-show').toggle();
        $tr.find('.editting-unshow').toggle();
    }

    var addError = function ($td, errMsg) {
        $td.append('<span class="help-block field-validation-error">' + errMsg + '</span>');
    }

    var removeError = function ($td) {
        var $error = $td.find('.help-block');
        if ($error) {
            $error.remove();
        }
    }

    var validateEmail = function (email) {
        var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    }

    var validate = function ($row) {        

        var isValid = true;

        var $tdName = $row.find('.td-editable[data-name="name"]');
        removeError($tdName);

        var $txtName = $tdName.find('.form-control');
        if (!$txtName.val()) {
            isValid = false;
            addError($tdName, 'Name is required.');
        }

        var $tdEmail = $row.find('.td-editable[data-name="email"]');
        removeError($tdEmail);

        var $txtEmail = $tdEmail.find('.form-control');
        if (!$txtEmail.val()) {
            isValid = false;
            addError($tdEmail, 'Email is required.');
        } else {
            if (!validateEmail($txtEmail.val())) {
                addError($tdEmail, 'Email address is invalid.');
            }
        }

        var $tdDateJoined = $row.find('.td-editable[data-name="dateJoined"]');
        removeError($tdDateJoined);

        var $txtDateJoined = $tdDateJoined.find('.form-control');
        if (!$txtDateJoined.val()) {
            isValid = false;
            addError($tdDateJoined, 'Date joined is required.');
        }

        return isValid;
    }

    var onEditClick = function () {
        $table.on('click', '.td-actions .btn-edit', function () {
            var $tr = $(this).closest('tr');
            var id = $tr.data('id');

            $tr.find('.td-editable').each(function () {
                var val = $(this).data('val');
                if ($(this).data('name') === 'dateJoined') {
                    var inputHtml = '<input class="form-control date-picker" type="text" value="' + val + '" />';
                } else {
                    var inputHtml = '<input class="form-control" type="text" value="' + val + '" />';
                }
                
                $(this).html(inputHtml);
            });

            $tr.find('.date-picker').datepicker({
                format: "M dd, yyyy"
            });

            toggerActionButtons($tr);
        });
    }

    var onSaveClick = function () {
        $table.on('click', '.td-actions .btn-save', function () {
            var $tr = $(this).closest('tr');

            if (!validate($tr)) {
                return false;
            }

            var id = $tr.data('id');

            var data = {
                id: id
            };

            $tr.find('.td-editable').each(function () {
                var $input = $(this).find('.form-control');
                var val = $input.val();
                var name = $(this).data('name');
                data[name] = val;
            });

            var url = id ? '/customers/' + id : '/customers';

            $.ajax({
                url: url,
                type: id ? 'PUT' : 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(data),
                success: function (response) {
                    onSaveSuccess(data, $tr);
                    toggerActionButtons($tr);

                    $.toast({
                        heading: 'Success',
                        text: 'The record has been saved successfully.',
                        showHideTransition: 'slide',
                        icon: 'success',
                        allowToastClose: true,
                        position: 'top-right'
                    });
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(textStatus, errorThrown);
                }
            });
        });
    }

    var onSaveSuccess = function (data, $tr) {

        $tr.data('id', data.id);

        $tr.find('.td-editable').each(function () {
            var name = $(this).data('name');
            $(this).data('val', data[name]);
            $(this).html(data[name]);
        });
    }

    var onCancelClick = function () {
        $table.on('click', '.td-actions .btn-cancel', function () {
            var $tr = $(this).closest('tr');
            var id = $tr.data('id');

            $tr.find('.td-editable').each(function () {
                var val = $(this).data('val');
                $(this).html(val);
            });

            toggerActionButtons($tr);
        });
    }


    var onDeleteClick = function () {
        $table.on('click', '.td-actions .btn-delete', function () {
            var $tr = $(this).closest('tr');

            Swal({
                title: 'Are you sure?',
                text: 'You will not be able to recover this action!',
                type: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Yes',
                cancelButtonText: 'No'
            }).then((result) => {
                if (result.value) {

                    var id = $tr.data('id');
                    if (!id) {
                        $tr.remove();
                    } else {
                        // call api delete

                        $.ajax({
                            url: '/customers/' + id,
                            type: 'DELETE',
                            contentType: 'application/json; charset=utf-8',
                            success: function (response) {
                                $tr.remove();
                                $.toast({
                                    heading: 'Success',
                                    text: 'The record has been deleted successfully.',
                                    showHideTransition: 'slide',
                                    icon: 'success',
                                    allowToastClose: true,
                                    position: 'top-right'
                                });
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                console.log(textStatus, errorThrown);
                            }
                        });
                    }

                    //Swal(
                    //    'Deleted!',
                    //    'Your imaginary file has been deleted.',
                    //    'success'
                    //)
                    // For more information about handling dismissals please visit
                    // https://sweetalert2.github.io/#handling-dismissals
                }
            })
            
        });
    }

    var onAddClick = function () {
        $table.on('click', '.btn-add', function () {
            var $newRow = $rowTemplate.clone();
            $newRow.removeClass('tr-template');
            //toggerActionButtons($newRow);

            $newRow.find('.btn-save').closest('.btn-container').show();
            $newRow.find('.btn-delete').closest('.btn-container').show();
            $newRow.find('.btn-edit').closest('.btn-container').hide();
            $newRow.find('.btn-cancel').closest('.btn-container').hide();

            $newRow.find('.date-picker').datepicker({
                format: "M dd, yyyy"
            });

            $rowTemplate.before($newRow);
        });
    }

    return {
        init: function () {
            $table = $('#user-table');
            $rowTemplate = $table.find('.tr-template:first');

            onSaveClick();
            onEditClick();
            onCancelClick();
            onDeleteClick();
            onAddClick();
        }

    };

}();