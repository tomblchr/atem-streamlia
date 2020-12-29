export async function apiTransition(): Promise<Response> {
    const options: RequestInit = {
        method: "POST"
    };
    return fetch("/api/atemconnection/transition", options);
};
