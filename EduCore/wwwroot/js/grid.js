document.addEventListener("DOMContentLoaded", function () {

    const table = document.querySelector(".ec-grid");

    if (!table)
        return;

    const selectAll =
        document.getElementById("selectAll");

    const checkboxes =
        table.querySelectorAll(".row-select");

    const toolbar =
        document.getElementById("selectionToolbar");

    const selectedCount =
        document.getElementById("selectedCount");

    function updateSelection() {

        const selected =
            table.querySelectorAll(".row-select:checked");

        selectedCount.textContent = selected.length;

        if (selected.length > 0)
            toolbar.classList.remove("d-none");
        else
            toolbar.classList.add("d-none");

        checkboxes.forEach(cb => {

            cb.closest("tr")
                .classList.remove("table-primary");

        });

        selected.forEach(cb => {

            cb.closest("tr")
                .classList.add("table-primary");

        });

    }

    checkboxes.forEach(cb => {

        cb.addEventListener("change", updateSelection);

    });

    if (selectAll) {

        selectAll.addEventListener("change", function () {

            checkboxes.forEach(cb => {

                cb.checked = this.checked;

            });

            updateSelection();

        });

    }

});