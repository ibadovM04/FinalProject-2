document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.delete').forEach(function (element) {
        element.addEventListener('click', function (event) {
            //event.preventDefault();
            let userId = this.getAttribute('data-id');
            console.log(userId)
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
                    fetch(`/User/Delete/${userId}`, {
                        method: 'DELETE'
                    }).then(response => {
                        if (response.ok) {
                            Swal.fire(
                                'Deleted!',
                                'Your product has been deleted.',
                                'success'
                            ).then(() => {
                                location.reload();
                            });
                        } else {
                            Swal.fire(
                                'Error!',
                                'There was an error deleting the product.',
                                'error'
                            );
                        }
                    });
                }
            });
        });
    });
});
