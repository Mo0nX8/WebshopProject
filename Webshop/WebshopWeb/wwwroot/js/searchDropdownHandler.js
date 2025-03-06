function searchSuggestion() {
    var searchValue = document.getElementById("search").value;
    if (searchValue.length >= 2) {
        $.ajax({
            url: getSearchSuggestionsUrl,
            type: 'GET',
            data: { searchValue: searchValue },
            success: function (data) {
                var suggestionsContainer = $('#suggestions-container');
                suggestionsContainer.empty();
                if (data && data.length > 0) {
                    data.forEach(function (item) {
                        var suggestion = `
                            <a href="/Product/Details/${item.id}" class="dropdown-item d-flex align-items-center">
                                <img src="${item.base64Image}" class="rounded me-2" width="50" height="50" alt="${item.productName}">
                                <span>${item.productName}</span>
                            </a>
                        `;
                        suggestionsContainer.append(suggestion);
                    });
                    suggestionsContainer.show();
                } else {
                    suggestionsContainer.hide();
                }
            },
            error: function () {
                
            }
        });
    } else {
        $('#suggestions-container').hide();
    }
}
