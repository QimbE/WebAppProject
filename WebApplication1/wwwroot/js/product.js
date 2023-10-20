var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/product/getall', dataSrc: "" },
        "columns": [
            { data: "title", "width": "25%" },
            { data: "isbn", "width": "15%" },
            { data: "listPrice", "width": "5%" },
            { data: "author", "width": "20%" },
            {
                data: "categories",
                "render": function (data) {
                    var res = data.map(function(category) {
                            return category.name;
                        }).join(', ');
                    return res;
                },
                "width": "10%"
            },
            {
                data: "id",
                "render": function(data) {
                    return `<div class=" btn-group" role="group">
                        <a href="/admin/product/upsert?id=${data}" class="btn btn-primary">
                            <i class="bi bi-pencil-square"></i> Edit
                        </a>
                        <a  onClick=Delete("/admin/product/delete/${data}") class="btn btn-danger">
                            <i class="bi bi-trash"></i> Delete
                        </a>
                    </div>`
                },
                "width": "20%"
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
