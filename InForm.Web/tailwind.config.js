/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "wwwroot/index.html",
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

