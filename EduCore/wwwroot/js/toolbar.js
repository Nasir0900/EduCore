document.addEventListener("DOMContentLoaded", function () {

    const toolbar =
        document.getElementById("selectionToolbar");

    if (!toolbar)
        return;

    document.addEventListener("keydown", function (e) {

        if (e.key === "Escape") {

            toolbar.classList.add("d-none");

            document
                .querySelectorAll(".row-select")
                .forEach(cb => {

                    cb.checked = false;

                    cb.closest("tr")
                        .classList.remove("table-primary");

                });

            const counter =
                document.getElementById("selectedCount");

            if (counter)
                counter.textContent = "0";

        }

    });

});