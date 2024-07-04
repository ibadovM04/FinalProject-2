function deleteProduct(productId) {
    if (confirm("Are you sure you want to delete this product? This action cannot be undone.")) {
        var xhr = new XMLHttpRequest();
        xhr.open("DELETE", `/Product/Delete?id=${productId}`, true);
        xhr.setRequestHeader('Content-Type', 'application/json');
        xhr.setRequestHeader('X-CSRF-TOKEN', document.querySelector('input[name="__RequestVerificationToken"]').value);
        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4) {
                if (xhr.status === 200) {
                    alert('Product has been deleted successfully.');
                    location.reload();
                } else {
                    alert('There was an error deleting the product.');
                }
            }
        };
        xhr.send();
    }
}