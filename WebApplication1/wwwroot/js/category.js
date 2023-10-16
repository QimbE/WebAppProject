var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function formatDate(date) {
    const options = {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit',
        second: '2-digit',
        hour12: false,
    };

    return new Date(date).toLocaleString(undefined, options);
}

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/category/getall', dataSrc: "" },
        "columns": [
            { data: "name", "width": "35%" },
            {
                data: "createdDateTime",
                "render": function (data) {
                    return formatDate(data);
                }, "width": "35%" },
            {
                data: "id",
                "render": function (data) {
                    return `<div class=" btn-group" role="group">
                        <a href="/admin/category/upsert?id=${data}" class="btn btn-primary">
                            <i class="bi bi-pencil-square"></i> Edit
                        </a>
                        <a  onClick=Delete("/admin/category/delete/${data}") class="btn btn-danger">
                            <i class="bi bi-trash"></i> Delete
                        </a>
                    </div>`
                },
                "width": "30%"
            }
        ]
    });
};

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}