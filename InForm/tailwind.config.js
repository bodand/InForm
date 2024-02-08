/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "App.razor",
        "./Features/**/*.{razor,html,cshtml}",
        "./Components/**/*.{razor,html,cshtml}"
    ],
    theme: {
        extend: {},
    },
    plugins: [
        require('@tailwindcss/forms')
    ],
}

