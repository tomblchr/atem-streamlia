import { toast as toastify } from 'react-toastify';

export function debug(message: string) {
    toastify.info(message);
    console.debug(message);
}

export function info(message: string): void {
    toastify.info(message);
    console.info(message);
}

export function warn(message: string): void {
    toastify.warn(message);
    console.warn(message);
}

export function error(message?: any, ...optionalParams: any[]): void {
    toastify.error(message);
    console.log(message, optionalParams);
}

export function log(message?: any, ...optionalParams: any[]): void {
    console.log(message, optionalParams);
}