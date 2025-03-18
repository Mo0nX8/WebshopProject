document.addEventListener("DOMContentLoaded", function () {
    const apiKey = "677345bee1294122b83150953252602";
    const searchBtn = document.getElementById("weather-search-btn");
    const searchInput = document.getElementById("weather-search");
    const weatherDisplay = document.getElementById("weather-display");

    getWeather("Gyongyos");

    searchBtn.addEventListener("click", function () {
        const city = searchInput.value.trim();
        if (city !== "") {
            getWeather(city);
        }
    });

    async function getWeather(city) {
        try {
            const response = await fetch(
                `https://api.weatherapi.com/v1/current.json?key=${apiKey}&q=${city}&lang=hu`
            );
            if (!response.ok) {
                throw new Error("Nem sikerült lekérni az adatokat.");
            }
            const data = await response.json();
            displayWeather(data);
        } catch (error) {
            weatherDisplay.innerHTML = `<p class="error">${error.message}</p>`;
        }
    }

    function displayWeather(data) {
        const cityName = data.location.name;
        const temp = data.current.temp_c;
        const description = data.current.condition.text;
        const icon = data.current.condition.icon;

        weatherDisplay.innerHTML = `
            <div style="display: flex; align-items: center; gap: 10px;">
                <img src="https:${icon}" alt="${description}" width="50">
                <div>
                    <strong>${cityName}</strong>: ${temp}°C, ${description}
                </div>
            </div>
        `;
    }
});