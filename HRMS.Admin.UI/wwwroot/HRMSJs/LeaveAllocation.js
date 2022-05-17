function Delete(id, eData) {

    CustomDeleteRecordLeave(id, "/LeaveAllocation/GetLeaveAllocationList", "/LeaveAllocation/DeleteLeaveAllocation", eData);
    function CustomDeleteRecordLeave(id, getUrl, deleteUrl, event) {
        alertify.set('notifier', 'position', 'top-right');

        alertify.confirm("Are you sure want to delete the record ?", function () {
           

            $('#divLoader').modal('show');

            $.get(deleteUrl, { id: id }, function (response) {

                alertify.success("Record deleted successfully");
                $('#divLoader').modal('hide');
            }).done(function () {

                location.reload();

                $(".form-control").val(''); 
            });
        }, function () {
            alertify.warning("You cancel the delete.");
        });
    }
}

function UpdateRecord(id) {
    UpdateCustomRecord(id, "/LeaveAllocation/CreateLeaveAllocation", "Update  Leave Allocation");
}