# README

This project is a sample solution for sending and receiving MQTT messages using .NET 10. The solution consists of two projects:

- **MqttSenders**: A console application that sends MQTT messages to a broker.
- **MqttReceivers**: A console application that receives MQTT messages from a broker.

## Prerequisites

- [Rancher Desktop](https://rancherdesktop.io/) or [Docker Desktop](https://www.docker.com/products/docker-desktop) or [Moby](https://github.com/moby/moby)

## Setup Environment

This project can be set up using either in GitHub Codespaces, Dev Containers, or directly with .NET 10.

### GitHub Codespaces

#### GitHub Codespaces: Usage

### Dev Containers

#### Dev Containers: Prerequisites

- [Node.js LTS](https://nodejs.org/en/)
- Install [Dev Containers extension](https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.remote-containers) in VSCode or the Dev Container CLI.

Dev Container CLI:

```bash
npm install -g @devcontainers/cli
```

#### Dev Containers: Usage

##### Dev Containers: Visual Studio Code

To start a Dev Container in VSCode, follow these steps:

1. Open the project folder in VSCode.
2. Press `F1` to open the command palette.
3. Type `Dev Containers: Reopen in Container` and select it.
4. VSCode will build the container and reopen the project inside the container.

For more information, refer to the [Dev Containers documentation](https://code.visualstudio.com/docs/remote/containers).

##### Dev Containers: CLI

To start a Dev Container using the CLI, follow these steps:

Run the following command in the root directory:

```bash
devcontainer up 
```

### Direct Usage

#### Direct Usage: Prerequisites

- Install [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) on your machine.

## Direct Usage: Debug and Run

### Direct Usage: Visual Studio Code

To debug and run the solution in Visual Studio Code, follow these steps:

1. Open the project folder in VSCode.
2. Press `F5` to start debugging.

### Direct Usage: Command Line

To debug and run the solution using the .NET CLI, follow these steps:

1. Open a terminal and navigate to the specific project directory (e.g. /src/MqttSenders).
2. Run the following command to start the project:

```bash
dotnet run
```
