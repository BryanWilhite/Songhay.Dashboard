module Songhay.Dashboard.Client.Visuals.Svg

open Bolero
open Bolero.Html

let svgData = Map [
    (
        "mdi_bolero_dance_24px",
        "M17 17H15V23H13V17H10.88L9.34 18.93L11.71 21.29L10.29 22.71L7.93 20.34C7.58 20 7.38 19.53 7.35 19.04C7.32 18.55 7.47 18.06 7.78 17.68L8.32 17H7L9 13V10C8.38 10.47 7.88 11.07 7.53 11.76C7.18 12.46 7 13.22 7 14H5C5 12.14 5.74 10.36 7.05 9.05C8.36 7.74 10.14 7 12 7C13.33 7 14.6 6.47 15.54 5.54C16.47 4.6 17 3.33 17 2H19C19 3.32 18.62 4.62 17.91 5.73C17.2 6.85 16.2 7.74 15 8.31V13L17 17M14 4C14 4.4 13.88 4.78 13.66 5.11C13.44 5.44 13.13 5.7 12.77 5.85C12.4 6 12 6.04 11.61 5.96C11.22 5.88 10.87 5.69 10.59 5.41C10.31 5.13 10.12 4.78 10.04 4.39C9.96 4 10 3.6 10.15 3.24C10.3 2.87 10.56 2.56 10.89 2.34C11.22 2.12 11.6 2 12 2C12.53 2 13.04 2.21 13.41 2.59C13.79 2.96 14 3.47 14 4Z"
    )
    (
        "mdi_codepen_24px",
        "M15.09,12L12,14.08V14.09L8.91,12L12,9.92V9.92L15.09,12M12,2C11.84,2 11.68,2.06 11.53,2.15L2.5,8.11C2.27,8.22 2.09,8.43 2,8.67V14.92C2,15.33 2,15.33 2.15,15.53L11.53,21.86C11.67,21.96 11.84,22 12,22C12.16,22 12.33,21.95 12.47,21.85L21.85,15.5C22,15.33 22,15.33 22,14.92V8.67C21.91,8.42 21.73,8.22 21.5,8.1L12.47,2.15C12.32,2.05 12.16,2 12,2M16.58,13L19.59,15.04L12.83,19.6V15.53L16.58,13M19.69,8.9L16.58,11L12.83,8.47V4.38L19.69,8.9M20.33,10.47V13.53L18.07,12L20.33,10.47M7.42,13L11.17,15.54V19.6L4.41,15.04L7.42,13M4.31,8.9L11.17,4.39V8.5L7.42,11L4.31,8.9M3.67,10.5L5.93,12L3.67,13.54V10.5Z"
    )
    (
        "mdi_github_circle_24px",
        "M12,2A10,10 0 0,0 2,12C2,16.42 4.87,20.17 8.84,21.5C9.34,21.58 9.5,21.27 9.5,21C9.5,20.77 9.5,20.14 9.5,19.31C6.73,19.91 6.14,17.97 6.14,17.97C5.68,16.81 5.03,16.5 5.03,16.5C4.12,15.88 5.1,15.9 5.1,15.9C6.1,15.97 6.63,16.93 6.63,16.93C7.5,18.45 8.97,18 9.54,17.76C9.63,17.11 9.89,16.67 10.17,16.42C7.95,16.17 5.62,15.31 5.62,11.5C5.62,10.39 6,9.5 6.65,8.79C6.55,8.54 6.2,7.5 6.75,6.15C6.75,6.15 7.59,5.88 9.5,7.17C10.29,6.95 11.15,6.84 12,6.84C12.85,6.84 13.71,6.95 14.5,7.17C16.41,5.88 17.25,6.15 17.25,6.15C17.8,7.5 17.45,8.54 17.35,8.79C18,9.5 18.38,10.39 18.38,11.5C18.38,15.32 16.04,16.16 13.81,16.41C14.17,16.72 14.5,17.33 14.5,18.26C14.5,19.6 14.5,20.68 14.5,21C14.5,21.27 14.66,21.59 15.17,21.5C19.14,20.16 22,16.42 22,12A10,10 0 0,0 12,2Z"
    )
    (
        "mdi_rss_24px",
        "M6.18,15.64A2.18,2.18 0 0,1 8.36,17.82C8.36,19 7.38,20 6.18,20C5,20 4,19 4,17.82A2.18,2.18 0 0,1 6.18,15.64M4,4.44A15.56,15.56 0 0,1 19.56,20H16.73A12.73,12.73 0 0,0 4,7.27V4.44M4,10.1A9.9,9.9 0 0,1 13.9,20H11.07A7.07,7.07 0 0,0 4,12.93V10.1Z"
    )
    (
        "mdi_stack_overflow_24px",
        "M17.36,20.2V14.82H19.15V22H3V14.82H4.8V20.2H17.36M6.77,14.32L7.14,12.56L15.93,14.41L15.56,16.17L6.77,14.32M7.93,10.11L8.69,8.5L16.83,12.28L16.07,13.9L7.93,10.11M10.19,6.12L11.34,4.74L18.24,10.5L17.09,11.87L10.19,6.12M14.64,1.87L20,9.08L18.56,10.15L13.2,2.94L14.64,1.87M6.59,18.41V16.61H15.57V18.41H6.59Z"
    )
]

let svgNode (viewBox: string) (pathData: string) =
    svg
        [
            "fill" => "currentColor"
            "fit" => ""
            "focusable" => "false"
            nameof viewBox => viewBox
            "preserveAspectRatio" => "xMidYMid meet"
            "xmlns" => "http://www.w3.org/2000/svg"
        ]
        [ RawHtml $@"<path d=""{pathData}""></path>" ]

let svgSpriteNode (href: string) (viewBox: string) =
    svg
        [
            "fill" => "currentColor"
            nameof viewBox => viewBox
            "xmlns" => "http://www.w3.org/2000/svg"
        ]
        [ RawHtml $@"<use href=""{href}""></use>" ]
