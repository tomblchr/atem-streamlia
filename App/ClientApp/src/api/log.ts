import { toast as toastify } from 'react-toastify';
import { ILogger, LogLevel } from "@microsoft/signalr";

export class AtemLogger implements ILogger {

    level: LogLevel;

    constructor(level?: LogLevel) {
        this.level = level ?? LogLevel.Error;
    }

    log(logLevel: LogLevel, message: string): void {
        if (this.level <= logLevel) {
            switch (logLevel) {
                case LogLevel.Trace:
                case LogLevel.Debug: 
                    debug(message); break;
                case LogLevel.Information:
                    info(message); break;
                case LogLevel.Warning:
                    warn(message); break;
                case LogLevel.Error:
                case LogLevel.Critical:
                    error(message); break;
            }
        }
    }

}

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