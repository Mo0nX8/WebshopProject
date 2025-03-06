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
                        var suggestion = '<a href="/Product/Details/' + item.id + '" class="dropdown-item">' + item.productName + '</a>';
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
