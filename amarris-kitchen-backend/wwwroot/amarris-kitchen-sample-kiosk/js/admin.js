// =========================
// LOGIN
// =========================
function login() {
    const user = document.getElementById("user").value;
    const pass = document.getElementById("pass").value;

    if (user === "admin" && pass === "1234") {
        localStorage.setItem("admin", "true");
        location.href = "dashboard.html";
    } else {
        document.getElementById("error").innerText = "Wrong login";
    }
}

// =========================
// CHECK AUTH
// =========================
function checkAuth() {
    if (localStorage.getItem("admin") !== "true") {
        location.href = "login.html";
    }
}

// =========================
// DASHBOARD
// =========================
function loadDashboard() {
    checkAuth();

    let orders = JSON.parse(localStorage.getItem("orders")) || [];

    let totalSales = 0;

    orders.forEach(o => {
        totalSales += o.total;
    });

    document.getElementById("totalOrders").innerText = orders.length;
    document.getElementById("totalSales").innerText = "₱" + totalSales;
}

// =========================
// ORDERS PAGE
// =========================
function loadOrders() {
    checkAuth();

    let orders = JSON.parse(localStorage.getItem("orders")) || [];

    const box = document.getElementById("ordersList");
    box.innerHTML = "";

    if (orders.length === 0) {
        box.innerHTML = "<p>No orders yet</p>";
        return;
    }

    orders.forEach((o, i) => {
        box.innerHTML += `
            <div class="card">
                <h3>Queue: ${o.queue}</h3>
                <p>Total: ₱${o.total}</p>
                <p>Status: ${o.status || "Pending"}</p>

                <button onclick="completeOrder(${i})">Mark Done</button>
            </div>
        `;
    });
}

// =========================
// COMPLETE ORDER
// =========================
function completeOrder(index) {
    let orders = JSON.parse(localStorage.getItem("orders")) || [];

    orders[index].status = "Completed";

    localStorage.setItem("orders", JSON.stringify(orders));

    loadOrders();
}

// =========================
// REPORTS
// =========================
function loadReports() {
    checkAuth();

    let orders = JSON.parse(localStorage.getItem("orders")) || [];

    let html = "<h3>All Orders</h3>";

    orders.forEach(o => {
        html += `
            <p>
                ${o.queue} - ₱${o.total} - ${o.status}
            </p>
        `;
    });

    document.getElementById("reportBox").innerHTML = html;
}

// =========================
// LOGOUT
// =========================
function logout() {
    localStorage.removeItem("admin");
    location.href = "login.html";
}