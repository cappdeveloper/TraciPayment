app.filter('range', function () {
    return function (input, total, currentPage) {
        total = parseInt(total);
        if (total > 0) {
            currentPage = parseInt(currentPage);
            var begin = currentPage - 1;
            var end = begin + 5;

            if (end >= total) {
                begin = total < 5 ? 0 : total - 5;
                end = total;
            }
            for (var i = 1; i <= total; i++) {
                input.push(i.toString());
            }
            if (end != total) {
                input[end - 1] = input[end - 1] + ' ...';
            }
            var aa = input.slice(begin, end);
            return input.slice(begin, end);
        }
        return input;
    };
});