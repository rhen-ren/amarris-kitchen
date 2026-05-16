// =============================================
// AMARRI'S KITCHEN — SINGLE FRONTEND JS FILE
// LOCALSTORAGE CART SYSTEM (NO BACKEND REQUIRED)
// =============================================

// this is the users current orders
let currentOrder = {
    orderMode: "",
    items: []
};



// =========================
// CART HELPERS (IMPORTANT)
// =========================
function getCart() {
    return JSON.parse(localStorage.getItem("cart")) || [];
}

function saveCart(cart) {
    localStorage.setItem("cart", JSON.stringify(cart));
}

///=========================
/// UPDATE USERS ORDDER MODE
///=========================
function selectOrderMode(orderMode) {
    currentOrder.orderMode = orderMode;
    localStorage.setItem("selected_order_mode", orderMode);
    window.location.href = "menu.html";
}


// =========================
// INIT MENU PAGE
// =========================
function initMenu() {
    renderCategories().then(() => { renderProducts("Silog Meals"); });
    
    updateCartCount();
}

// =========================
// CATEGORY RENDER
// =========================
async function renderCategories() {

    const catDiv = document.getElementById("categories");
    if (!catDiv) return;

    //get categories in the backend
    const response = await fetch("/category");
    const categories = await response .json();


    catDiv.innerHTML = "";

    categories.forEach(cat => {
        const btn = document.createElement("button");
        btn.innerText = cat.categoryName;

        btn.onclick = () => renderProducts(cat.categoryName);

        catDiv.appendChild(btn);
    });
}

// =========================
// PRODUCT RENDER
// =========================

async function renderProducts(cat) {

    const div = document.getElementById("products");

    if (!div) return;

    div.innerHTML = "";

    div.classList.add("product-grid");

    //get products in the backend
    const response = await fetch(`/product/${cat}`)
    const products = await response.json();

    products.forEach(p => {

        const item = document.createElement("div");
        const imageSrc = p.image ? p.image : "https://placehold.co/300x200?text=Food";

        item.className = "product-card";

        item.innerHTML = `
        
            <img src="${imageSrc}"
                 class="product-image"
                 onerror="this.src='https://placehold.co/300x200?text=Food'">

            <h3>${p.productName}</h3>

            <p class="price">₱${p.unitPrice}</p>

            <button onclick="openModal('${p.productId}', '${p.productName}', ${p.unitPrice}, '${p.image}')">
                Add to Cart
            </button>

        `;

        div.appendChild(item);
    });
}

// =========================
// ADD TO CART
// =========================
// function addToCart(name, price) {
//     let cart = getCart();

//     cart.push({ name, price });

//     saveCart(cart);

//     updateCartCount();
// }

// modalfunctions
let currentSelectedItem = null;

function openModal(id, name, price, img) {
    currentSelectedItem = { id, name, price, img };

    document.getElementById("modalName").innerText = name;
    document.getElementById("modalPrice").innerText = "₱" + price;
    document.getElementById("modalImg").src = img || 'https://placehold.co/300x200?text=Food';
    document.getElementById("modalQty").value = 1;

    document.getElementById("productModal").style.display = "flex";
}

function closeModal() {
    document.getElementById("productModal").style.display = "none";
}

function updateModalQty(amt) {
    const qtyInput = document.getElementById("modalQty");
    let currentQty = parseInt(qtyInput.value);
    let newQty = currentQty + amt;
    if (newQty < 1) newQty = 1;
    qtyInput.value = newQty;
}

function confirmAddToCart() {
    const qty = parseInt(document.getElementById("modalQty").value);
    let cart = getCart();

    const currentId = Number(currentSelectedItem.id);

    const existingIndex = cart.findIndex(item => Number(item.id) === currentId);

    if (existingIndex > -1) {
        cart[existingIndex].qty += qty;
    } else {
        cart.push({
            id: currentSelectedItem.id,
            name: currentSelectedItem.name,
            price: currentSelectedItem.price,
            qty: qty
        });
    }

    saveCart(cart);
    updateCartCount();
    closeModal();
}



// =========================
// CART COUNT BADGE
// =========================
function updateCartCount() {
    const cart = getCart();
    const totalItems = cart.reduce((sum, item) => sum + item.qty, 0);
    const el = document.getElementById("cartCount");

    if (el) {
        el.innerText = totalItems;
    }
}

// =========================
// CART PAGE LOAD
// =========================
function loadCart() {
    const cart = getCart();

    const container = document.getElementById("cartItems");
    if (!container) return;

    container.innerHTML = "";

    let grandTotal = 0;

    cart.forEach((item, index) => {
        const itemSubTotal = item.price * item.qty;
        grandTotal += itemSubTotal;

        container.innerHTML += `
            <div style="border:1px solid #ccc; padding:10px; margin:10px;">
                <h3>${item.name}<span class="text-orange"> × ${item.qty}</span></h3>
                <p>₱${itemSubTotal}</p>
                <button onclick="removeItem(${index})">Remove</button>
            </div>
        `;
    });

    const totalEl = document.getElementById("cartTotal");
    if (totalEl) totalEl.innerText = "₱" + grandTotal;
}

// ============================
// update users order items
// ============================
function updateUsersOrderItems() {
    const rawUiCart = getCart() || [];

    const selectedMode = localStorage.getItem("selected_order_mode") || "Dine-in";
    currentOrder.orderMode = selectedMode;
    currentOrder.items = rawUiCart.map(cartItem => ({
        productId: cartItem.id,
        qty: cartItem.qty
    }));

    localStorage.setItem("kiosk_order", JSON.stringify(currentOrder));
}


// =========================
// REMOVE ITEM
// =========================
function removeItem(index) {
    let cart = getCart();

    cart.splice(index, 1);

    saveCart(cart);

    loadCart();
    updateCartCount();
}

// =========================
// GO TO CART
// =========================
function goCart() {
    location.href = "cart.html";
}

// =========================
// CHECKOUT PAGE
// =========================
function goCheckout() {
    const rawUiCart = getCart() || [];
    if (rawUiCart.length === 0) {
        alert("Your Cart is empty! Please add items before checking out.");
        return;
    }

    updateUsersOrderItems();
    const savedOrder = localStorage.getItem("kiosk_order");
    alert(JSON.stringify(JSON.parse(savedOrder), null, 2));

    location.href = "checkout.html";
}

//load the checkout summary
function loadCheckoutSummary() {
    const cart = getCart();
    const container = document.getElementById("checkoutSummary");

    if (!container) return;
    container.innerHTML = "";

    let grandTotal = 0

    cart.forEach((item) => {
        const itemSubTotal = item.price * item.qty;
        grandTotal += itemSubTotal;


        container.innerHTML += `<div style="display: flex; justify-content: space-between; margin-bottom: 8px; font-size: 1.1rem;">
        <span>${item.name}<strong style="color: #ff6600;">× ${item.qty}</strong></span>
        <span>₱${itemSubTotal.toFixed(2)}</span>
        </div>
        `

    });

    const vatableSales = grandTotal / 1.12;
    const extractedVat = grandTotal - vatableSales;

    document.getElementById("grossSubtotal").innerText = "₱" + grandTotal.toFixed(2);
    document.getElementById("vatAmount").innerText = "₱" + extractedVat.toFixed(2);
    document.getElementById("netTotal").innerText = "₱" + grandTotal.toFixed(2);
}

async function toPayment() {
    const finalPayload = JSON.parse(localStorage.getItem("kiosk_order"));

    if (!finalPayload || !finalPayload.items || finalPayload.items.length === 0) {
        alert("ERROR. Please restart your ordering process.");
        return;
    }

    //create the order in the database
    try {
        const response = await fetch('http://localhost:5250/order/create', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(finalPayload)
        });

        if (response.ok) {
            const serverResult = await response.json();

            localStorage.removeItem("cart");
            localStorage.removeItem("kiosk_order");
            location.href = "payment.html";
        }

    }
    catch (error) {
        console.error(error);
        alert("Could not reach the server.")
    }
    
}
// =========================
// QUEUE GENERATOR (SAFE A-Z)
// =========================
let prefixIndex = 0;
let counter = 1;

function generateQueue() {
    const letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    const queue = `${letters[prefixIndex]}-${String(counter).padStart(3, "0")}`;

    counter++;

    if (counter > 999) {
        counter = 1;
        prefixIndex++;

        if (prefixIndex >= letters.length) {
            prefixIndex = 0; // reset after Z
        }
    }

    return queue;
}

// =========================
// CASH PAYMENT
// =========================
function cashPayment() {
    const queue = generateQueue();

    localStorage.setItem("queue", queue);
    localStorage.setItem("payment", "cash");

    saveOrder(queue);

    location.href = "success.html";
}

// =========================
// ONLINE PAYMENT (MOCK QR)
// =========================
function onlinePayment() {
    const qr = document.getElementById("qrBox");
    if (qr) qr.style.display = "block";

    setTimeout(() => {
        const queue = generateQueue();

        localStorage.setItem("queue", queue);
        localStorage.setItem("payment", "online");

        saveOrder(queue);

        location.href = "success.html";
    }, 3000);
}

// =========================
// SUCCESS SCREEN
// =========================
function loadSuccess() {
    const queue = localStorage.getItem("queue");
    const payment = localStorage.getItem("payment");

    const q = document.getElementById("queue");
    const p = document.getElementById("payment");

    if (q) q.innerText = "Queue Number: " + queue;

    if (p) {
        if (payment === "cash") {
            p.innerText = "Please proceed to counter for payment";
        } else {
            p.innerText = "Paid Online (Mock QR)";
        }
    }
}

// =========================
// RESET ORDER
// =========================
function newOrder() {
    localStorage.clear();
    location.href = "start.html";
}

// =========================

function saveOrder(queue) {
    let cart = JSON.parse(localStorage.getItem("cart")) || [];

    let total = 0;

    cart.forEach(item => {
        total += item.price;
    });

    let orders = JSON.parse(localStorage.getItem("orders")) || [];

    orders.push({
        queue: queue,
        total: total,
        status: "Pending",
        items: cart
    });

    localStorage.setItem("orders", JSON.stringify(orders));
}







