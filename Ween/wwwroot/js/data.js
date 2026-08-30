// ween — shared data & state
// State that must survive across separate page loads (logged-in user,
// reservations, admin-added categories/listings, uploaded photos) is kept
// in localStorage. Seed data below stays constant; additions layer on top.

const SEED_CITIES = [
  { slug: "amman", name: "Amman", ar: "عمّان", count: 214, tint: "#2D6E7E" },
  { slug: "irbid", name: "Irbid", ar: "إربد", count: 76, tint: "#A8452F" },
  { slug: "zarqa", name: "Zarqa", ar: "الزرقاء", count: 58, tint: "#3D5A47" },
  { slug: "aqaba", name: "Aqaba", ar: "العقبة", count: 92, tint: "#1D5C73" },
  { slug: "salt", name: "As-Salt", ar: "السلط", count: 34, tint: "#7A4A2B" },
  { slug: "madaba", name: "Madaba", ar: "مادبا", count: 41, tint: "#8B3A3A" },
  { slug: "jerash", name: "Jerash", ar: "جرش", count: 29, tint: "#556B4E" },
  { slug: "karak", name: "Karak", ar: "الكرك", count: 22, tint: "#6B4E3D" },
];

const SEED_CATEGORIES = [
  { slug: "coffee", name: "Coffee Shops", icon: "☕", tint: "#A8452F" },
  { slug: "restaurants", name: "Restaurants", icon: "🍽", tint: "#2D6E7E" },
  { slug: "hotels", name: "Hotels", icon: "🛏", tint: "#C89B3C" },
  { slug: "transport", name: "Transport", icon: "🚌", tint: "#3D5A47" },
];

const RESERVATION_FIELDS = {
  coffee: { verb: "Reserve a table", partyLabel: "Guests", showRoom: false },
  restaurants: { verb: "Reserve a table", partyLabel: "Guests", showRoom: false },
  hotels: { verb: "Book a room", partyLabel: "Guests", showRoom: true },
  transport: { verb: "Reserve a seat", partyLabel: "Passengers", showRoom: false },
};

const ADMIN_FIELDS = {
  coffee: [{ id: "seating", label: "Seating capacity", type: "text", placeholder: "e.g. 30" }, { id: "wifi", label: "Wifi available", type: "select", options: ["Yes", "No"] }],
  restaurants: [{ id: "cuisine", label: "Cuisine type", type: "text", placeholder: "e.g. Levantine, Grill" }, { id: "seating", label: "Seating capacity", type: "text", placeholder: "e.g. 60" }],
  hotels: [{ id: "rooms", label: "Number of rooms", type: "text", placeholder: "e.g. 40" }, { id: "stars", label: "Star rating", type: "select", options: ["1", "2", "3", "4", "5"] }],
  transport: [{ id: "routes", label: "Routes served", type: "text", placeholder: "e.g. Amman – Irbid, Amman – Aqaba" }, { id: "vehicle", label: "Vehicle type", type: "text", placeholder: "e.g. Coach bus, Minivan" }],
};
function adminFieldsFor(slug) { return ADMIN_FIELDS[slug] || [{ id: "notes", label: "Notes", type: "text", placeholder: "Anything specific to this category" }]; }

const SEED_LISTINGS = [
  { id: "coffee-amman-1", city: "amman", category: "coffee", name: "Rumi Coffee House", tag: "Cozy", rating: 4.8, address: "Rainbow Street, Jabal Amman", hours: "8:00 AM – 11:00 PM", phone: "+962 6 461 2233", icon: "☕", desc: "A quiet corner off Rainbow Street with hand-poured Arabica and a reading nook upstairs.", locationUrl: "", photos: [] },
  { id: "coffee-amman-2", city: "amman", category: "coffee", name: "Sindibad Café", tag: "Budget", rating: 4.4, address: "Shmeisani, Amman", hours: "7:00 AM – 12:00 AM", phone: "+962 6 566 8890", icon: "☕", desc: "A long-running neighborhood favorite known for its karak tea and fast service.", locationUrl: "", photos: [] },
  { id: "coffee-amman-3", city: "amman", category: "coffee", name: "Wild Jordan Café", tag: "View", rating: 4.7, address: "Al Weibdeh, Amman", hours: "9:00 AM – 10:00 PM", phone: "+962 6 463 3542", icon: "☕", desc: "Perched on the edge of Jabal Amman with a terrace view over the old city.", locationUrl: "", photos: [] },
  { id: "rest-amman-1", city: "amman", category: "restaurants", name: "Beit Sitti", tag: "Traditional", rating: 4.9, address: "Al Weibdeh, Amman", hours: "12:00 PM – 10:00 PM", phone: "+962 6 462 1111", icon: "🍽", desc: "Home-style Jordanian cooking classes and dinner service in a restored family house.", locationUrl: "", photos: [] },
  { id: "rest-amman-2", city: "amman", category: "restaurants", name: "Fakhreldin", tag: "Fine dining", rating: 4.6, address: "Jabal Amman, 1st Circle", hours: "1:00 PM – 11:00 PM", phone: "+962 6 465 2399", icon: "🍽", desc: "A grand Ottoman-era villa serving Levantine classics.", locationUrl: "", photos: [] },
  { id: "rest-amman-3", city: "amman", category: "restaurants", name: "Hashem Restaurant", tag: "Local legend", rating: 4.5, address: "Downtown Amman", hours: "24 hours", phone: "+962 6 463 0968", icon: "🍽", desc: "No menu, no chairs to spare — just falafel, hummus, and foul since 1956.", locationUrl: "", photos: [] },
  { id: "hotel-amman-1", city: "amman", category: "hotels", name: "Landmark Amman Hotel", tag: "Business", rating: 4.5, address: "Abdali, Amman", hours: "Check-in from 3:00 PM", phone: "+962 6 560 7100", icon: "🛏", desc: "A tower hotel in the new downtown, close to Abdali Boulevard.", locationUrl: "", photos: [] },
  { id: "hotel-amman-2", city: "amman", category: "hotels", name: "Toledo Hotel", tag: "Boutique", rating: 4.7, address: "Jabal Amman, 3rd Circle", hours: "Check-in from 2:00 PM", phone: "+962 6 465 7777", icon: "🛏", desc: "A quiet boutique stay a short walk from the Rainbow Street cafés.", locationUrl: "", photos: [] },
  { id: "trans-amman-1", city: "amman", category: "transport", name: "Tabarbour Bus Station", tag: "Intercity", rating: 4.0, address: "Tabarbour, North Amman", hours: "5:00 AM – 9:00 PM", phone: "+962 6 500 1122", icon: "🚌", desc: "Main departure point for buses north to Irbid and Jerash.", locationUrl: "", photos: [] },
  { id: "trans-amman-2", city: "amman", category: "transport", name: "Wehdat Station (South)", tag: "Intercity", rating: 3.9, address: "Al Wehdat, Amman", hours: "5:30 AM – 8:00 PM", phone: "+962 6 477 4410", icon: "🚌", desc: "Departures south toward Karak, Aqaba, and Madaba.", locationUrl: "", photos: [] },
  { id: "coffee-irbid-1", city: "irbid", category: "coffee", name: "Al Yasmeen Café", tag: "Student spot", rating: 4.3, address: "University Street, Irbid", hours: "7:00 AM – 1:00 AM", phone: "+962 2 727 4410", icon: "☕", desc: "Right by Yarmouk University — reliable wifi, cheap coffee.", locationUrl: "", photos: [] },
  { id: "rest-irbid-1", city: "irbid", category: "restaurants", name: "Al Quds Restaurant", tag: "Grill", rating: 4.4, address: "Downtown Irbid", hours: "11:00 AM – 11:00 PM", phone: "+962 2 724 5501", icon: "🍽", desc: "Mixed grill plates and fresh bread from a stone oven.", locationUrl: "", photos: [] },
  { id: "hotel-irbid-1", city: "irbid", category: "hotels", name: "Al Joude Hotel", tag: "Mid-range", rating: 4.2, address: "Baghdad Street, Irbid", hours: "Check-in from 2:00 PM", phone: "+962 2 725 5800", icon: "🛏", desc: "A straightforward, well-kept stay near the city center.", locationUrl: "", photos: [] },
  { id: "trans-irbid-1", city: "irbid", category: "transport", name: "Irbid New Station", tag: "Intercity", rating: 4.0, address: "New Amman Road, Irbid", hours: "5:00 AM – 9:00 PM", phone: "+962 2 727 1190", icon: "🚌", desc: "Main hub north of Amman, with regular departures to the capital.", locationUrl: "", photos: [] },
  { id: "coffee-zarqa-1", city: "zarqa", category: "coffee", name: "Zarqa Corner Café", tag: "Local", rating: 4.1, address: "King Talal Street, Zarqa", hours: "8:00 AM – 12:00 AM", phone: "+962 5 398 2211", icon: "☕", desc: "A dependable neighborhood café known for cardamom coffee.", locationUrl: "", photos: [] },
  { id: "rest-zarqa-1", city: "zarqa", category: "restaurants", name: "Al Bawadi Grill", tag: "Grill", rating: 4.3, address: "Zarqa Main Street", hours: "12:00 PM – 11:00 PM", phone: "+962 5 398 7744", icon: "🍽", desc: "Large portions and quick turnaround for family dinners.", locationUrl: "", photos: [] },
  { id: "coffee-aqaba-1", city: "aqaba", category: "coffee", name: "Red Sea Roastery", tag: "Sea view", rating: 4.6, address: "Corniche, Aqaba", hours: "7:00 AM – 11:00 PM", phone: "+962 3 201 4477", icon: "☕", desc: "Beachfront seating with a view across to Eilat — best at sunset.", locationUrl: "", photos: [] },
  { id: "rest-aqaba-1", city: "aqaba", category: "restaurants", name: "Ali Baba Restaurant", tag: "Seafood", rating: 4.5, address: "Aqaba Corniche", hours: "12:00 PM – 12:00 AM", phone: "+962 3 201 3901", icon: "🍽", desc: "A Corniche institution for grilled Red Sea catch.", locationUrl: "", photos: [] },
  { id: "hotel-aqaba-1", city: "aqaba", category: "hotels", name: "Captain's Hotel", tag: "Beachfront", rating: 4.4, address: "South Beach, Aqaba", hours: "Check-in from 3:00 PM", phone: "+962 3 201 8888", icon: "🛏", desc: "Simple rooms, private beach access, and an on-site dive center.", locationUrl: "", photos: [] },
  { id: "trans-aqaba-1", city: "aqaba", category: "transport", name: "Aqaba Bus Station", tag: "Intercity", rating: 3.8, address: "Aqaba", hours: "5:00 AM – 8:00 PM", phone: "+962 3 201 5566", icon: "🚌", desc: "Departures north to Amman and Wadi Rum transfers.", locationUrl: "", photos: [] },
  { id: "coffee-salt-1", city: "salt", category: "coffee", name: "Salt Heritage Café", tag: "Historic", rating: 4.5, address: "Old Salt, As-Salt", hours: "8:00 AM – 10:00 PM", phone: "+962 5 355 2200", icon: "☕", desc: "Set in a restored Ottoman-era house on the old souq steps.", locationUrl: "", photos: [] },
  { id: "rest-madaba-1", city: "madaba", category: "restaurants", name: "Haret Jdoudna", tag: "Traditional", rating: 4.6, address: "Near the Mosaic Map, Madaba", hours: "11:00 AM – 10:00 PM", phone: "+962 5 324 8650", icon: "🍽", desc: "Courtyard dining a short walk from the Madaba Mosaic Map.", locationUrl: "", photos: [] },
  { id: "hotel-jerash-1", city: "jerash", category: "hotels", name: "Hadrian Gate Hotel", tag: "Near ruins", rating: 4.2, address: "Jerash", hours: "Check-in from 2:00 PM", phone: "+962 2 635 1523", icon: "🛏", desc: "Walking distance from the Roman ruins.", locationUrl: "", photos: [] },
  { id: "trans-karak-1", city: "karak", category: "transport", name: "Karak Station", tag: "Intercity", rating: 3.7, address: "Karak", hours: "5:30 AM – 7:00 PM", phone: "+962 3 235 1002", icon: "🚌", desc: "Connects Karak to Amman and south toward Tafilah.", locationUrl: "", photos: [] },
];

const SEED_RESERVATIONS = [
  { id: "r1", listingId: "coffee-amman-1", name: "Yaseen Khattab", date: "2026-09-04", time: "17:00", party: "2", nights: null, status: "confirmed" },
  { id: "r2", listingId: "hotel-aqaba-1", name: "Yaseen Khattab", date: "2026-09-12", time: "15:00", party: "2", nights: "3", status: "confirmed" },
];

// ---------- localStorage-backed state ----------
const LS = {
  user: "mvcs_user",
  reservations: "mvcs_reservations",
  categoriesAdded: "mvcs_categories_added",
  listingsAdded: "mvcs_listings_added",
  citiesAdded: "mvcs_cities_added",
};

function lsGet(key, fallback) {
  try { const v = localStorage.getItem(key); return v ? JSON.parse(v) : fallback; }
  catch (e) { return fallback; }
}
function lsSet(key, value) {
  try { localStorage.setItem(key, JSON.stringify(value)); }
  catch (e) { /* storage unavailable — state just won't persist across pages */ }
}

function getCities() { return [...SEED_CITIES, ...lsGet(LS.citiesAdded, [])]; }
function addCityToStorage(city) {
  const added = lsGet(LS.citiesAdded, []);
  added.push(city);
  lsSet(LS.citiesAdded, added);
}

function getCategories() { return [...SEED_CATEGORIES, ...lsGet(LS.categoriesAdded, [])]; }
function addCategoryToStorage(cat) {
  const added = lsGet(LS.categoriesAdded, []);
  added.push(cat);
  lsSet(LS.categoriesAdded, added);
  RESERVATION_FIELDS[cat.slug] = { verb: "Reserve", partyLabel: "Guests", showRoom: false };
}

function getListings() { return [...SEED_LISTINGS, ...lsGet(LS.listingsAdded, [])]; }
function addListingToStorage(listing) {
  const added = lsGet(LS.listingsAdded, []);
  added.push(listing);
  lsSet(LS.listingsAdded, added);
}

function getUser() { return lsGet(LS.user, null); }
function setUser(user) { lsSet(LS.user, user); }
function clearUser() { try { localStorage.removeItem(LS.user); } catch (e) {} }

function getReservations() { return lsGet(LS.reservations, SEED_RESERVATIONS); }
function saveReservations(list) { lsSet(LS.reservations, list); }
function addReservation(res) {
  const list = getReservations();
  list.push(res);
  saveReservations(list);
}
function cancelReservationById(id) {
  const list = getReservations();
  const r = list.find(x => x.id === id);
  if (r) r.status = "cancelled";
  saveReservations(list);
}

// ---------- lookups ----------
function qs(name) { return new URLSearchParams(window.location.search).get(name); }
function findCity(slug) { return getCities().find(c => c.slug === slug); }
function findCategory(slug) { return getCategories().find(c => c.slug === slug); }
function findListing(id) { return getListings().find(l => l.id === id); }
function listingsFor(citySlug, categorySlug) { return getListings().filter(l => l.city === citySlug && l.category === categorySlug); }
function categoryCountsFor(citySlug) {
  const counts = {};
  getCategories().forEach(cat => { counts[cat.slug] = getListings().filter(l => l.city === citySlug && l.category === cat.slug).length; });
  return counts;
}

// ---------- top nav (shared across every page) ----------
function initTopnav() {
  const el = document.getElementById('topnav-account');
  if (!el) return;
  const user = getUser();
  el.textContent = user ? user.name.split(' ')[0] : 'Log in';
  el.href = user ? 'account.html' : 'login.html';
}
document.addEventListener('DOMContentLoaded', initTopnav);
