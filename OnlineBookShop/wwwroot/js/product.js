var dataTable;

$(document).ready(function () {
   loadDataTable(); 
});


function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url:'/admin/product/getall'},
        "columns": [
            { data: 'title', "width": "25%" },
            { data: 'isbn', "width": "15%" },
            { data: 'listPrice', "width": "10%" },
            { data: 'author', "width": "15%" },
            { data: 'category.name', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                     <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>               
                     <a onClick=Delete('/admin/product/delete/${data}') class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
                    </div>`
                },
                "width": "25%"
            }
        ]
    });
}


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
    
// attributes are case sensitive
// follow that shown in the json format
/**
 * {
 *   "data": [
 *     {
 *       "id": 1,
 *       "title": "Fortune of Time",
 *       "isbn": "SWD9999001",
 *       "description": "<p>Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies. Nunc malesuada viverra ipsum sit amet tincidunt.</p>",
 *       "author": "Billy Spark",
 *       "listPrice": 99,
 *       "price": 90,
 *       "price50": 85,
 *       "price100": 80,
 *       "categoryId": 2,
 *       "category": {
 *         "id": 2,
 *         "name": "Sci-Fi",
 *         "displayOrder": 2
 *       },
 *       "imageUrl": "/images/product/66e20492-7804-4c2d-b7d7-f32aa387fd2f.jpg"
 *     }
 * }
 */
